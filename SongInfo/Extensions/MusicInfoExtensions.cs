using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.SongInfo.Extensions;

internal static class MusicInfoExtensions
{
    internal static string GetBundleFilename(this MusicInfo info)
    {
        if (info == null)
        {
            return string.Empty;
        }

        // MusicInfo has music as "{id}_music", e.g. "inferno_city_music"
        // its data (not only music) is stored in a bundle named "music_{id}_assets_all.bundle", e.g. "music_inferno_city_assets_all.bundle"

        // but for some reason _all_ instances of word "music" are stripped -- even if they are technically part of the song title;
        // so "computer_music_girl_music" is stored in "music_computer_girl_assets_all.bundle"
        return $"music_{info.music.Replace("_music", string.Empty)}_assets_all.bundle";
    }
}
