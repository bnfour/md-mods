using System.Collections.Generic;
using System.Text.RegularExpressions;

using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.SongInfo.Utilities;

/// <summary>
/// Converts music asset name to name of the bundle it's stored in.
/// </summary>
internal static class BundleFilenameConstructor
{
    private static readonly Regex FileNameCleaningRegex = new(@"_music$");

    // "music" in song name leads to non-standard naming for some reason
    private static readonly Dictionary<string, string> Exceptions = new()
    {
        ["39_music_music"] = "music_39_assets_all.bundle",
        ["computer_music_girl_music"] = "music_computer_girl_assets_all.bundle",
        ["enka_dance_music_music"] = "music_enka_dance_assets_all.bundle",
        ["dont_fight_the_music_music"] = "music_dont_fight_the_assets_all.bundle"
    };

    internal static string IdToFilename(MusicInfo info)
    {
        if (Exceptions.ContainsKey(info?.music))
        {
            return Exceptions[info?.music];
        }
        // MusicInfo has music as "{id}_music", e.g. "inferno_city_music"
        // its data (not only music) is stored in a bundle named "music_{id}_assets_all.bundle", e.g. "music_inferno_city_assets_all.bundle"
        return $"music_{FileNameCleaningRegex.Replace(info.music, string.Empty, 1)}_assets_all.bundle";
    }
}
