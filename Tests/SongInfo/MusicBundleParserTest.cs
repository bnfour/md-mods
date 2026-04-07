using Bnfour.MuseDashMods.SongInfo.Exceptions;
using Bnfour.MuseDashMods.SongInfo.Utilities;

namespace Bnfour.MuseDashMods.Tests.SongInfo;


public class MusicBundleParserTest
{
    [Fact]
    public void ParsingWorksInGeneral()
    {
        var rawDuration = new MusicBundleParser("./Data/MusicBundles/h.bundle").GetDuration();

        // raw duration data is 0x00872a40
        Assert.Equal(2.6644f, rawDuration, 0.0001f);
    }

    [Fact]
    public void ReportsWhenFloatInPreviousBlock()
    {
        // in this bundle, the "archive:/" substring we search by is at the beginning of the second block;
        // the duration float is near the end of an already discarded block,
        // so the naïve parser is unable to get its value

        Assert.Throws<VeryUnluckyException>(
            () => new MusicBundleParser("./Data/MusicBundles/h-torn-1.bundle").GetDuration(),
            ex => ex.BundleName == "h-torn-1.bundle" && ex.Message == "The duration float is in the previous block."
                ? null
                : "unexpected exception data"
        );
    }

    [Fact]
    public void ReportsWhenUnableToFindAnchorString()
    {
        // in this bundle, the "archive:/" is split between the blocks:
        // the first one ends with "arch", and the second one starts with "ive:/",
        // so the naïve parser is unable to find it at all

        Assert.Throws<VeryUnluckyException>(
            () => new MusicBundleParser("./Data/MusicBundles/h-torn-2.bundle").GetDuration(),
            ex => ex.BundleName == "h-torn-2.bundle" && ex.Message == "Unable to locate the anchor string -- split between blocks?"
                ? null
                : "unexpected exception data"
        );
    }
}
