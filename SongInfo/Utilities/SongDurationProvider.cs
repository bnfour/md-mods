using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MelonLoader;
using Newtonsoft.Json;
using UnityEngine;

using Il2CppAssets.Scripts.Database;
using Il2CppPeroTools2.Resources;

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
        using (var embeddedDataStream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream(EmbeddedDataName))
        {
            using (var reader = new StreamReader(embeddedDataStream))
            {
                var raw = reader.ReadToEnd();
                var dataOnly = JsonConvert.DeserializeObject<Dictionary<string, string>>(raw);
                _internalData = new(dataOnly, new MusicInfoUidComparer());
            }
        }

        var overrideFullPath = Path.Combine(Application.dataPath, OverrideFilename);
        if (File.Exists(overrideFullPath))
        {
            try
            {
                using (var reader = new StreamReader(overrideFullPath))
                {
                    var raw = reader.ReadToEnd();
                    var dataOnly = JsonConvert.DeserializeObject<Dictionary<string, string>>(raw);
                    _overrideCache = new(dataOnly, new MusicInfoUidComparer());
                }
            }
            catch (JsonException)
            {
                // TODO a warning here would be nice
                // but this is (probably) called when the logger is still unavailable
                // (that was the case with the scoreboard characters before)
                _overrideCache = new(new MusicInfoUidComparer());
            }
        }
        else
        {
            _overrideCache = new(new MusicInfoUidComparer());
        }

        if (_overrideCache.Count > 0)
        {
            // removes all override entries that match the (updated) data,
            // because those were probably generated with not up-to-date version of the mod
            var toRemove = new List<string>();
            foreach (var kvp in _overrideCache)
            {
                if (_internalData.ContainsKey(kvp.Key) && _internalData[kvp.Key] == kvp.Value)
                {
                    toRemove.Add(kvp.Key);
                }
            }
            foreach (var key in toRemove)
            {
                _overrideCache.Remove(key);
            }
        }
    }

    public string GetDuration(MusicInfo info)
    {
        if (_overrideCache.ContainsKey(info.uid))
        {
            return _overrideCache[info.uid];
        }
        else if (_internalData.ContainsKey(info.uid))
        {
            return _internalData[info.uid];
        }
        else
        {
            var duration = FormatDuration(GetDurationDirectly(info));
            _overrideCache[info.uid] = duration;

            Melon<SongInfoMod>.Logger.Warning($"Apologies for the lag, needed to get duration data not present in the cache.");

            return duration;
        }
    }

    public void Shutdown()
    {
        // save the updated overrides back to the file,
        // or remove the file if it's present, but no overrides remain
        var overrideFullPath = Path.Combine(Application.dataPath, OverrideFilename);
        if (_overrideCache.Count == 0)
        {
            if (File.Exists(overrideFullPath))
            {
                File.Delete(overrideFullPath);
            }
        }
        else
        {
            using (var writer = new StreamWriter(overrideFullPath, false))
            {
                writer.Write(JsonConvert.SerializeObject(_overrideCache, Formatting.Indented));
            }
        }
    }

    // these are only used outside of this class in debug-only developer mode,
    // no need for them to be accessible outside this class in a release build

#if DEBUG
    public static string FormatDuration(float rawDuration)
#else
    private static string FormatDuration(float rawDuration)
#endif
    {
        return $"{(int)(rawDuration / 60):00}:{(int)(rawDuration % 60):00}";
    }

#if DEBUG
    public static float GetDurationDirectly(MusicInfo info)
#else
    private static float GetDurationDirectly(MusicInfo info)
#endif
    {
        // this is a time-consuming operation (usualy 200~300 ms for me, which is noticeable)
        // so it is avoided whenever possible by precollecting the data

        // AudioClips in the game are not set up for loading metadata only first (why would they?),
        // so the entire file is loaded, hence the delay

        var ac = ResourcesManager.instance.LoadFromName<AudioClip>(info.music);
        return ac.length;

        // it _should_ be okay to not dispose of the loaded clip,
        // as there is a big chance it's going to be played straight away
        // ...i hope the resources manager is smarter than i am >v<

    }
}
