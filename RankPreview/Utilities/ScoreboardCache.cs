using System.Collections.Generic;
using System.Linq;

namespace Bnfour.MuseDashMods.RankPreview.Utilities;

internal class ScoreboardCache
{
    // good enough to hold scoreboards for a few recently opened songs
    private const int Capacity = 8;
    private readonly LimitedCapacityDictionary<string, int[]> _backend = new(Capacity);

    internal void Store(string key, IEnumerable<int> value)
    {
        _backend[key] = value.ToArray();
    }

    // TODO do we want this (getting rank, formatting rank) as a single method?
    // currently supposed to return the finalized value to display
    // -- handles missing entries and certainty (more on that later) internally
    internal string EstimateRank(string key, int currentScore)
    {
        if (_backend.ContainsKey(key))
        {
            var estimatedRank = _backend[key].TakeWhile(score => score >= currentScore).Count() + 1;

            if (estimatedRank > 99)
            {
                return "#100+";
            }

            // TODO extra formatting?

            return $"#{estimatedRank}";
        }
        else
        {
            return "¯\\_(ツ)_/¯";
        }
    }
}
