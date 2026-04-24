using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

using K4os.Compression.LZ4;

using Bnfour.MuseDashMods.SongInfo.Extensions;
using Bnfour.MuseDashMods.SongInfo.Exceptions;

namespace Bnfour.MuseDashMods.SongInfo.Utilities;

internal class MusicBundleParser(string bundlePath)
{
    // the actual duration float starts 20 bytes earlier
    // might break if more than one AudioClip (or non-code asset in general) is ever packed into a bundle
    private static readonly byte[] Pattern = Encoding.ASCII.GetBytes("archive:/");

    // used to report parsing errors via exceptions
    private readonly string _bundleFilename = Path.GetFileName(bundlePath);

    internal float GetDuration()
    {
        // assume the file exists, check is done before calling
        using (var bundleFileStreamReader = new StreamReader(bundlePath))
        {
            var bundleFileStream = bundleFileStreamReader.BaseStream;
            // parse the header
            var fileMagic = bundleFileStream.ReadString();
            ThrowIfNot(fileMagic == "UnityFS");

            var bundleVersion = bundleFileStream.ReadInt();
            ThrowIfNot(bundleVersion == 7);

            var webPlayerVersion = bundleFileStream.ReadString();
            ThrowIfNot(webPlayerVersion == "5.x.x");

            var unityVersion = bundleFileStream.ReadString();
            ThrowIfNot(unityVersion[0..7] == "2019.4.");

            var fileSize = bundleFileStream.ReadLong();
            ThrowIfNot(fileSize == new FileInfo(bundlePath).Length);

            var compressedDataSize = bundleFileStream.ReadInt();
            var decompressedDataSize = bundleFileStream.ReadInt();
            ThrowIfNot(compressedDataSize <= decompressedDataSize);

            var flags = bundleFileStream.ReadInt();
            // 0x43 = block and directory info combined, LZ4HC compression
            ThrowIfNot(flags == 0x43);

            bundleFileStream.AlignTo0x10Forward();

            var compressedMetadataBuffer = new byte[compressedDataSize];
            var bytesRead = bundleFileStream.Read(compressedMetadataBuffer, 0, compressedMetadataBuffer.Length);
            ThrowIfNot(bytesRead == compressedMetadataBuffer.Length);
            var decompressedMetadataBuffer = new byte[decompressedDataSize];

            var decompressedBytesWritten = LZ4Codec.Decode(compressedMetadataBuffer, 0, compressedDataSize, decompressedMetadataBuffer, 0, decompressedDataSize);
            ThrowIfNot(decompressedBytesWritten == decompressedDataSize);
            // read blocks metadata, skip dir info metadata
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

                    // LZ4HC or nothing, others are not supported
                    ThrowIfNot(blockFlags == 3 || blockFlags == 0);

                    blocksMeta.Add((blockDecompressedSize, blockCompressedSize, blockFlags));
                }
                // we don't really care about directory info also present in metadata block
            }
            foreach (var tuple in blocksMeta)
            {
                var decompressedBuffer = new byte[tuple.decompressedSize];
                if (tuple.flags == 3)
                {
                    var compressedBuffer = new byte[tuple.compressedSize];
                    bytesRead = bundleFileStream.Read(compressedBuffer, 0, compressedBuffer.Length);
                    ThrowIfNot(bytesRead == compressedBuffer.Length);
                    decompressedBytesWritten = LZ4Codec.Decode(compressedBuffer, 0, compressedBuffer.Length, decompressedBuffer, 0, decompressedBuffer.Length);
                    ThrowIfNot(decompressedBytesWritten == decompressedBuffer.Length);
                }
                else
                {
                    ThrowIfNot(tuple.flags == 0);
                    bytesRead = bundleFileStream.Read(decompressedBuffer, 0, decompressedBuffer.Length);
                    ThrowIfNot(bytesRead == decompressedBuffer.Length);
                }
                var idx = decompressedBuffer.AsSpan().IndexOf(Pattern);
                if (idx != -1)
                {
                    // we might get giga unlucky -- the meta we're searching for
                    // has a very slim chance to be spread between two blocks
                    // (so far, none of the game's bundles had this)
                    // while i do test for such cases, i have no plans to implement anything to mitigate this
                    // until a vanilla game bundle is formed that way
                    if (idx < 20)
                    {
                        throw new VeryUnluckyException("The duration float is in the previous block.", _bundleFilename);
                    }
                    return BinaryPrimitives.ReadSingleLittleEndian(decompressedBuffer.AsSpan()[(idx - 20)..(idx - 16)]);
                }
            }
            throw new VeryUnluckyException("Unable to locate the anchor string -- split between blocks?", _bundleFilename);
        }
    }

    private void ThrowIfNot(bool condition)
    {
        if (!condition)
        {
            throw new BundleParseException(_bundleFilename);
        }
    }
}
