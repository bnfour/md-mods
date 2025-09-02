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

            // a top 8 score that is immediately visible is a "!!!" for me
            var impressiveness = estimatedRank switch
            {
                <= 8 => "!!!",
                <= 20 => "!!",
                <= 50 => "!",
                _ => string.Empty
            };
            // how sure we are about the result, depending on total number of scoreboard entries
            var certainty = _backend[key].Length switch
            {
                0 => "???",
                <= 50 => "??",
                <= 90 => "?",
                _ => string.Empty
            };

            return $"#{estimatedRank}{impressiveness}{certainty}";
        }
        else
        {
            return "¯\\_(ツ)_/¯";
        }
    }
}
