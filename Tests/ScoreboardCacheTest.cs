using Bnfour.MuseDashMods.RankPreview.Utilities;

namespace Bnfour.MuseDashMods.Tests;

public class ScoreboardCacheTest
{
    private readonly ScoreboardCache _cache;

    public ScoreboardCacheTest()
    {
        _cache = new();
    }

    [Fact]
    public void CapacityIsStillLimited()
    {
        for (var i = 0; i < 9; i++)
        {
            var pseudoKey = $"0-{i}_1";
            _cache.Store(pseudoKey, [12345, 1234, 123, 12, 1]);
        }

        Assert.Equal("¯\\_(ツ)_/¯", _cache.EstimateRank("0-0_1", 100));

        for (var i = 1; i < 9; i++)
        {
            var pseudoKey = $"0-{i}_1";
            Assert.Equal("#2!!!??", _cache.EstimateRank(pseudoKey, 10000));
        }

        Assert.Equal("¯\\_(ツ)_/¯", _cache.EstimateRank("0-9_1", 100));
    }

    // TODO also test
    // - special value for #100+
    // - impressiveness and certainty
    // - actual ranking
}
