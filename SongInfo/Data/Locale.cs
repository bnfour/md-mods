using MelonLoader;

using Il2CppAssets.Scripts.Database;
using Il2CppPeroPeroGames.GlobalDefines;

namespace Bnfour.MuseDashMods.SongInfo.Data;

/// <summary>
/// Used to provide localized string to use in mod UI.
/// </summary>
/// <remarks>See <see cref="Language">GlobalDefines.Language</see> for details
/// on how the game handles languages internally.</remarks>
internal static class Locale
{
    private static readonly string[] _lengthLocalizations = new[]
    {
        "[duration]", // dummy "none" entry
        "Length", // English
        "时长", // Chinese Simplified
        "長度", // Chinese Traditional
        "時間", // Japanese
        "길이" // Korean
    };

    /// <summary>
    /// The term for song length (duration?) in the language the game currently uses.
    /// </summary>
    internal static string Length
    {
        get
        {
            // TODO can also check that Language.language_count matches our array's length

            var index = Language.LanguageToIndex(DataHelper.userLanguage);
            // LanguageToIndex returns -1 for unknown strings
            if (index < 0)
            {
                Melon<SongInfoMod>.Logger.Warning($"Unknown language \"{DataHelper.userLanguage}\" (index {index}), defaulting to {_lengthLocalizations[0]}");
                index = 0;
            }

            return _lengthLocalizations[index];
        }
    }
}
