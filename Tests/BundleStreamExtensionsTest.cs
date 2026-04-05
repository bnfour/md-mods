namespace Bnfour.MuseDashMods.Tests;

using Bnfour.MuseDashMods.SongInfo.Extensions;

public class BundleStreamExtensionsTest
{
    private readonly MemoryStream stream = new();

    [Fact]
    public void AligningWorks()
    {
        const int max = 33;
        stream.Write(Enumerable.Range(0, max).Select(i => (byte)i).ToArray());
        for (int i = 0; i < max; i++)
        {
            stream.Seek(i, SeekOrigin.Begin);
            stream.AlignTo0x10Forward();

            Assert.Equal(0, stream.Position % 0x10);
            Assert.True(stream.Position >= i);
        }
    }

    [Fact]
    public void ReadStringWorks()
    {
        string[] strings = ["fOrSeN", "FoRsEn", "i'm your number one fan", string.Empty];
        // the last \0 is intentionally omitted, the code takes any value < 1 (0 for actual string termination, -1 for stream end) as a string end
        stream.Write([102, 79, 114, 83, 101, 78, 0, 70, 111, 82, 115, 69, 110, 0, 105, 39, 109, 32, 121, 111, 117, 114, 32, 110, 117, 109, 98, 101, 114, 32, 111, 110, 101, 32, 102, 97, 110]);
        stream.Seek(0, SeekOrigin.Begin);
        foreach (var s in strings)
        {
            Assert.Equal(s, stream.ReadString());
        }
    }

    [Theory]
    [InlineData(new byte[] { 0, 0 }, 0)]
    [InlineData(new byte[] { 0x00, 0x07 }, 7)]
    [InlineData(new byte[] { 0x01, 0x23 }, 0x0123)]
    [InlineData(new byte[] { 0xc0, 0xde }, -16_162)]
    [InlineData(new byte[] { 0xff, 0xff }, -1)]
    [InlineData(new byte[] { 0xff, 0xfe }, -2)]
    [InlineData(new byte[] { 0x7f, 0xff }, short.MaxValue)]
    [InlineData(new byte[] { 0x80, 0x00 }, short.MinValue)]
    public void ReadShortWorks(byte[] raw, short expected)
    {
        stream.Write(raw);
        stream.Seek(0, SeekOrigin.Begin);

        Assert.Equal(expected, stream.ReadShort());
    }

    [Theory]
    [InlineData(new byte[] { 0, 0, 0, 0 }, 0)]
    [InlineData(new byte[] { 0, 0, 0, 7 }, 7)]
    [InlineData(new byte[] { 0x01, 0x23, 0x45, 0x67 }, 0x01234567)]
    [InlineData(new byte[] { 0xc0, 0xde, 0xff, 0xff }, -1_059_127_297)]
    [InlineData(new byte[] { 0xff, 0xff, 0xff, 0xff }, -1)]
    [InlineData(new byte[] { 0xff, 0xff, 0xff, 0x01 }, -255)]
    [InlineData(new byte[] { 0x7f, 0xff, 0xff, 0xfe }, int.MaxValue - 1)]
    [InlineData(new byte[] { 0x80, 0x00, 0x00, 0x00 }, int.MinValue)]
    public void ReadIntWorks(byte[] raw, int expected)
    {
        stream.Write(raw);
        stream.Seek(0, SeekOrigin.Begin);

        Assert.Equal(expected, stream.ReadInt());
    }

    [Theory]
    [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 0)]
    [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 0, 7 }, 7)]
    [InlineData(new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef }, 0x0123456789abcdef)]
    [InlineData(new byte[] { 0xc0, 0xde, 0xff, 0xff, 0x67, 0x67, 0x67, 0x67 }, -4_548_917_101_181_048_985)]
    [InlineData(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, -1)]
    [InlineData(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xf0, 0x0f }, -4_081)]
    [InlineData(new byte[] { 0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, long.MaxValue)]
    [InlineData(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, long.MinValue + 1)]
    public void ReadLongWorks(byte[] raw, long expected)
    {
        stream.Write(raw);
        stream.Seek(0, SeekOrigin.Begin);

        Assert.Equal(expected, stream.ReadLong());
    }


    [Fact]
    public void ReadNumbersThrowOnInsufficientData()
    {
        stream.Write([0xdd]);

        stream.Seek(0, SeekOrigin.Begin);
        Assert.Throws<InvalidOperationException>(() => { var xdd = stream.ReadShort(); });

        stream.Seek(0, SeekOrigin.Begin);
        Assert.Throws<InvalidOperationException>(() => { var xdd = stream.ReadInt(); });

        stream.Seek(0, SeekOrigin.Begin);
        Assert.Throws<InvalidOperationException>(() => { var xdd = stream.ReadLong(); });
    }

}
