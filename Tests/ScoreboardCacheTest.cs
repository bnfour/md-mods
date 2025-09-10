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

    // TODO also test
    // - special value for #100+
    // - impressiveness and certainty
    // - actual ranking
}
