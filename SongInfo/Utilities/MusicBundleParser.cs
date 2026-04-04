using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using K4os.Compression.LZ4;

using Bnfour.MuseDashMods.SongInfo.Extensions;

namespace Bnfour.MuseDashMods.SongInfo.Utilities;

internal class MusicBundleParser(string Path)
{
    internal float GetDurationFast()
    {
        // assume the file exists, check is done before calling
        using (var bundleFileStreamReader = new StreamReader(Path))
        {
            var bundleFileStream = bundleFileStreamReader.BaseStream;
            // parse the header
            var fileMagic = bundleFileStream.ReadString();
            Debug.Assert(fileMagic == "UnityFS");

            var bundleVersion = bundleFileStream.ReadInt();
            Debug.Assert(bundleVersion == 7);

            var webPlayerVersion = bundleFileStream.ReadString();
            Debug.Assert(webPlayerVersion == "5.x.x");

            var unityVersion = bundleFileStream.ReadString();
            Debug.Assert(unityVersion[0..7] == "2019.4.");

            var fileSize = bundleFileStream.ReadLong();
            Debug.Assert(fileSize == new FileInfo(Path).Length);

            var compressedDataSize = bundleFileStream.ReadInt();
            var decompressedDataSize = bundleFileStream.ReadInt();

            var flags = bundleFileStream.ReadInt();
            Debug.Assert(flags == 0x43);

            bundleFileStream.AlignTo0x10Forward();

            // TODO check actually read lengths everywhere ReadExactly was replaced

            var compressedMetadataBuffer = new byte[compressedDataSize];
            bundleFileStream.Read(compressedMetadataBuffer, 0, compressedMetadataBuffer.Length);
            var decompressedMetadataBuffer = new byte[decompressedDataSize];

            var decompressedBytesWritten = LZ4Codec.Decode(compressedMetadataBuffer, 0, compressedDataSize, decompressedMetadataBuffer, 0, decompressedDataSize);
            Debug.Assert(decompressedBytesWritten == decompressedDataSize);
            // read blocks metadata, skip dir info metadata -- meta goes before raw data anyway
            var blocksMeta = new List<(int decompressedSize, int compressedSize, short flags)>();
            using (var metadataStream = new MemoryStream(decompressedMetadataBuffer))
            {
                // skip hash (zeroed anyway?)
                metadataStream.Seek(16, SeekOrigin.Begin);
                var blockCount = metadataStream.ReadInt();
                for (int i = 0; i < blockCount; i++)
                {
                    var blockDecompressedSize = metadataStream.ReadInt();
                    var blockCompressedSize = metadataStream.ReadInt();
                    var blockFlags = metadataStream.ReadShort();

                    // Lz4HC or nothing, others are not supported
                    Debug.Assert(blockFlags == 3 || blockFlags == 0);

                    blocksMeta.Add((blockDecompressedSize, blockCompressedSize, blockFlags));
                }

                var dirinfoCount = metadataStream.ReadInt();
                for (int j = 0; j < dirinfoCount; j++)
                {
                    metadataStream.Seek(sizeof(long) + sizeof(long) + sizeof(int), SeekOrigin.Current);
                    metadataStream.SkipString();
                }
            }
            foreach (var tuple in blocksMeta)
            {
                var decompressedBuffer = new byte[tuple.decompressedSize];
                if (tuple.flags == 3)
                {
                    var compressedBuffer = new byte[tuple.compressedSize];
                    bundleFileStream.Read(compressedBuffer, 0, compressedBuffer.Length);
                    decompressedBytesWritten = LZ4Codec.Decode(compressedBuffer, 0, compressedBuffer.Length, decompressedBuffer, 0, decompressedBuffer.Length);
                    Debug.Assert(decompressedBytesWritten == decompressedBuffer.Length);
                }
                else
                {
                    Debug.Assert(tuple.flags == 0);
                    bundleFileStream.Read(decompressedBuffer, 0, decompressedBuffer.Length);
                }
                var wellKnownPattern = Encoding.ASCII.GetBytes("archive:/");
                var idx = decompressedBuffer.AsSpan().IndexOf(wellKnownPattern);
                if (idx != -1)
                {
                    // we might get giga unlucky -- the meta we're searching for
                    // has a very slim chance to be spread between two blocks
                    // (so far, none of the game's bundles had this)
                    if (idx < 20)
                    {
                        // forsenSWA
                        throw new ApplicationException("very unlucky -- meta is split between blocks");
                    }
                    var f = BinaryPrimitives.ReadSingleLittleEndian(decompressedBuffer.AsSpan()[(idx - 20)..(idx - 16)]);
                    return f;
                }
            }
            throw new ApplicationException("very unlucky -- unable to process the bundle");
        }
    }
}
