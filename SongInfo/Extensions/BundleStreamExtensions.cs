using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Bnfour.MuseDashMods.SongInfo.Extensions;

/// <summary>
/// Extension methods used to read binary asset bundle file.
/// </summary>
internal static class BundleStreamExtensions
{
    // too bad this runtime does not allow to use generic ReadOnlySpan
    private static T Read<T>(Stream s, Func<byte[], T> converter)
    {
        var buffer = new byte[Marshal.SizeOf<T>()];
        // too bad ReadExactly was introduced in .NET 7
        // TODO debug assert we read the number of bytes we wanted
        s.Read(buffer, 0, buffer.Length);
        return converter(buffer);
    }

    internal static void AlignTo0x10Forward(this Stream s)
    {
        var currentPosition = s.Position;
        if (currentPosition % 16 != 0)
        {
            s.Seek(16 - currentPosition % 16, SeekOrigin.Current);
        }
    }

    internal static string ReadString(this Stream s)
    {
        var bytes = new List<byte>();
        int b;
        while ((b = s.ReadByte()) > 0)
        {
            bytes.Add((byte)b);
        }
        return Encoding.ASCII.GetString([.. bytes]);
    }

    internal static void SkipString(this Stream s)
    {
        while ((_ = s.ReadByte()) != 0) { }
    }

    internal static short ReadShort(this Stream s)
        => Read(s, array => BinaryPrimitives.ReadInt16BigEndian(array.AsSpan()));

    internal static int ReadInt(this Stream s)
        => Read(s, array => BinaryPrimitives.ReadInt32BigEndian(array.AsSpan()));

    internal static long ReadLong(this Stream s)
        => Read(s, array => BinaryPrimitives.ReadInt64BigEndian(array.AsSpan()));
}
