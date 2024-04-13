using System;
using System.Collections.Generic;
using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.SongInfo.Utilities;

/// <summary>
/// Handles getting the song duration from multiple sources,
/// designed to minimize disruptions (lags).
/// </summary>
public class SongDurationProvider
{
    private const string EmbeddedDataName = "Bnfour.MuseDashMods.SongInfo.Resources.duration_data.json";
    private const string OverrideFilename = "song_info_override.json";

    private readonly SortedList<string, string> _internalData;
    private readonly SortedList<string, string> _overrideCache;

    public SongDurationProvider()
    {
        // TODO load data, both internal and external (if present)
    }

    public string GetDuration(MusicInfo info)
    {
        // TODO the overengineered dual-cache super omega system EX
        throw new NotImplementedException("soon™");
    }

    public void Shutdown()
    {
        // TODO prune* and re-save external cache
        // *"prune" stands for "remove any entries that duplicate those in the main data"
        //   should be useful after a patch if the game was played before the updated mod version was installed
    }

    private string FormatDuration(float rawDuration)
    {
        return $"{(int)(rawDuration / 60):00}:{(int)(rawDuration % 60):00}";
    }

    private float GetDurationDirectly(MusicInfo info)
    {
        // this is a time-consuming operation (usualy 200~300 ms for me, which is noticeable)
        // so it is avoided whenever possible by precollecting the data
        throw new NotImplementedException("soon™");
    }
}
