using Bnfour.MuseDashMods.SongInfo.Utilities;

namespace Bnfour.MuseDashMods.Tests;

public class MusicInfoUidComparerTest
{
    private readonly MusicInfoUidComparer _comparer;
    private static readonly Random _random = new();

    public MusicInfoUidComparerTest()
    {
        _comparer = new();
    }

    [Theory]
    [InlineData("forsen", "nesrof")]
    [InlineData("10-0", "nesrof")]
    [InlineData("forsen", "10-0")]
    [InlineData("10-0", "10-0-0")]
    [InlineData("10.5-0", "10-0")]
    [InlineData("10-0", "10-")]
    [InlineData("-0", "10-0")]
    public void DoesNotCareAboutNonUids(string first, string second)
    {
        Assert.Equal(0, _comparer.Compare(first, second));
    }

    [Theory]
    [InlineData(0, "3-2", "3-2")]
    [InlineData(1, "0-1", "0-0")]
    [InlineData(1, "0-10", "0-0")]
    [InlineData(1, "1-0", "0-0")]
    [InlineData(1, "1-0", "0-99")]
    [InlineData(1, "10-1", "2-1")]
    [InlineData(1, "12-10", "12-2")]
    [InlineData(1, "12-20", "12-2")]
    public void DoesCareAboutUids(int expectedSign, string first, string second)
    {
        Assert.Equal(expectedSign, Math.Sign(_comparer.Compare(first, second)));
        Assert.Equal(-expectedSign, Math.Sign(_comparer.Compare(second, first)));
    }

    [Fact]
    public void ActuallyWorksAsIntended()
    {
        // initial sorted data to compare to
        var list = new List<string>()
        {
            "0-0", "0-1", "0-3", "0-10", "0-101",
            "1-0", "1-1", "1-4", "1-10", "1-22",
            "3-0", "3-1", "3-10", "3-100", "4-0"
        };

        var extras = new[] { "99-0", "99-99" };
        // add big elements to the beginning so that the test won't break
        // even if the random returned the list in the same order by some miracle
        var shuffled = extras.Concat(list.OrderBy(x => _random.Next()));

        // actually order data
        var compared = shuffled.Order(_comparer);
        // assert it was ordered correctly
        Assert.Equal(list.Concat(extras), compared);
    }
}
