using System;
using System.Collections.Generic;

namespace Bnfour.MuseDashMods.RankPreview.Utilities;

internal class ScoreboardCache
{
    internal void Store(string key, IEnumerable<int> value)
    {
        // TODO
    }

    // TODO do we want this (getting rank, formatting rank) as a single method?
    // currently supposed to return the finalized value to display
    // -- handles missing entries and certainty (more on that later) internally
    internal string EstimateRank(string key, int currentScore)
    {
        return "xddShrug";
    }
}
