using System.Text.RegularExpressions;

using Bnfour.MuseDashMods.RankPreview.Utilities;

namespace Bnfour.MuseDashMods.Tests;

public partial class ScoreboardCacheTest
{
    private readonly ScoreboardCache _cache;
    private readonly Random _random;

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumberRegex();

    [GeneratedRegex(@"#([1-9][0-9]?|100\+)!{0,3}\?{0,3}", RegexOptions.ExplicitCapture)]
    private static partial Regex ValidEstimationRegex();

    public ScoreboardCacheTest()
    {
        _cache = new();
        _random = new();
    }

    [Fact]
    public void CapacityIsStillLimited()
    {
        for (var i = 0; i < 8; i++)
        {
            var pseudoKey = $"0-{i}_1";
            _cache.Store(pseudoKey, [12345, 1234, 123, 12, 1]);
        }
        // another update to test a bit further
        // as 0-0 is updated, 0-1 is the first to go
        _cache.Store("0-0_1", [23456, 2345, 234, 23, 2]);
        _cache.Store("0-8_1", [34567, 3456, 345, 34, 3]);

        Assert.Equal("¯\\_(ツ)_/¯", _cache.EstimateRank("0-1_1", 100));

        for (var i = 0; i < 9; i++)
        {
            if (i == 1)
            {
                continue;
            }

            var pseudoKey = $"0-{i}_1";
            Assert.Equal("#2!!!??", _cache.EstimateRank(pseudoKey, 10000));
        }

        Assert.Equal("¯\\_(ツ)_/¯", _cache.EstimateRank("0-9_1", 100));
    }

    [Theory]
    [InlineData("a")]
    [InlineData("80-2_3")]
    public void ShrugsOnUnknownUid(string unknown)
    {
        Assert.Equal("¯\\_(ツ)_/¯", _cache.EstimateRank(unknown, 100));
    }

    [Theory]
    [InlineData(100)]
    [InlineData(101)]
    [InlineData(256)]
    [InlineData(1999)]
    public void HundredPlusActuallyWorks(int scoreboardLength)
    {
        // TODO are we ok with arbitrary strings as keys?
        // something more typed?
        var scoreboard = Enumerable.Repeat(12345, scoreboardLength);
        _cache.Store("test key please ignore", scoreboard);

        Assert.Equal("#100+", _cache.EstimateRank("test key please ignore", 10000));
    }

    [Theory]
    [InlineData(3, 1, 8)]
    [InlineData(2, 9, 20)]
    [InlineData(1, 21, 50)]
    [InlineData(0, 51, 200)]
    public void ImpressivenessWorksAsIntended(int expectedExclamationCount, int rankFrom, int rankTo)
    {
        for (int i = rankFrom; i <= rankTo; i++)
        {
            var scoreboard = Enumerable.Repeat(12345, i - 1);
            _cache.Store("a", scoreboard);

            Assert.Equal(expectedExclamationCount, _cache.EstimateRank("a", 10000).Count(c => c == '!'));
        }
    }

    [Theory]
    [InlineData(3, 0, 0)]
    [InlineData(2, 1, 50)]
    [InlineData(1, 51, 90)]
    [InlineData(0, 91, 200)]
    public void CertaintyWorksAsIntended(int expectedQuestionCount, int rankFrom, int rankTo)
    {
        for (int i = rankFrom; i <= rankTo; i++)
        {
            var scoreboard = Enumerable.Range(0, i).Select(i => _random.Next());
            _cache.Store("b", scoreboard);
            // we actually don't care what rank it is and only check its certainty based on total count
            Assert.Equal(expectedQuestionCount, _cache.EstimateRank("b", 10000).Count(c => c == '?'));
        }
    }

    [Theory]
    [InlineData(1, new int[] { }, 100)]
    [InlineData(1, new int[] { 50, 48 }, 100)]
    [InlineData(2, new int[] { 100, 99 }, 100)]
    [InlineData(2, new int[] { 100 }, 100)]
    [InlineData(3, new int[] { 100, 100, 90 }, 100)]
    [InlineData(2, new int[] { 101, 90 }, 100)]
    [InlineData(3, new int[] { 101, 100, 90 }, 100)]
    [InlineData(7, new int[] { 107, 106, 106, 106, 100, 100, 90 }, 100)]
    public void RankingWorks(int expectedRank, IEnumerable<int> scoreboard, int score)
    {
        _cache.Store("xdd", scoreboard);
        var rawRank = _cache.EstimateRank("xdd", score);
        var rawNumber = NumberRegex().Match(rawRank).Value;

        Assert.True(int.TryParse(rawNumber, out int estimatedRank));
        Assert.Equal(expectedRank, estimatedRank);

        Assert.True(ValidEstimationRegex().Match(rawRank).Success);
    }

    [Fact]
    public void AlternateRankingTest()
    {
        // run a few times to be sure
        for (int i = 0; i < 3; i++)
        {
            var scoreboard = Enumerable.Range(0, 200)
                .Select(i => _random.Next(10_000, 1_000_000)).OrderByDescending(x => x)
                // keep it stable
                .ToArray();
            _cache.Store("okayeg", scoreboard);

            var score = _random.Next(10_000, 1_000_000);
            // remember the 100+ feature
            var expected = Math.Min(100,
                    Array.LastIndexOf([.. scoreboard.Append(score).OrderByDescending(x => x)], score) + 1);

            var rawRank = _cache.EstimateRank("okayeg", score);
            var rawNumber = NumberRegex().Match(rawRank).Value;

            Assert.True(int.TryParse(rawNumber, out int estimatedRank));
            Assert.Equal(expected, estimatedRank);
            // 200 entries => we're fairly certain
            Assert.Equal(0, rawRank.Count(c => c == '?'));

            Assert.True(ValidEstimationRegex().Match(rawRank).Success);
        }
    }
}
