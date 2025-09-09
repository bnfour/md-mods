using Bnfour.MuseDashMods.SongInfo.Utilities;

namespace Tests;

public class MusicInfoUidComparerTest
{
    private readonly MusicInfoUidComparer _comparer;
    private readonly Random _random;

    // helper aliases for positive test
    private static readonly Func<int, bool> Greater = (result) => result > 0;
    private static readonly Func<int, bool> Equal = (result) => result == 0;
    private static readonly Func<int, bool> Lesser = (result) => result < 0;

    public static readonly TheoryData<Func<int, bool>, string, string> DataForPositiveTests =
    [
        new(Equal, "3-2", "3-2"),

        new(Greater, "0-1", "0-0"),
        new(Greater, "0-10", "0-0"),
        new(Greater, "1-0", "0-0"),
        new(Greater, "1-0", "0-99"),
        new(Greater, "10-1", "2-1"),

        new(Greater, "12-10", "12-2"),
        new(Greater, "12-20", "12-2"),

        new(Lesser, "0-0", "0-1"),
        new(Lesser, "0-0", "0-10"),
        new(Lesser, "0-0", "1-0"),
        new(Lesser, "0-99", "1-0"),
        new(Lesser, "2-1", "10-1"),

        new(Lesser, "12-2", "12-10"),
        new(Lesser, "12-2", "12-20")
    ];

    public MusicInfoUidComparerTest()
    {
        _comparer = new();
        _random = new();
    }

    [Theory]
    [InlineData("forsen", "nesrof")]
    [InlineData("10-0", "nesrof")]
    [InlineData("forsen", "10-0")]
    public void DoesNotCareAboutNonUids(string first, string second)
    {
        Assert.Equal(0, _comparer.Compare(first, second));
    }

    [Theory]
    [MemberData(nameof(DataForPositiveTests))]
    public void DoesCareAboutUids(Func<int, bool> helper, string first, string second)
    {
        Assert.True(helper(_comparer.Compare(first, second)));
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
