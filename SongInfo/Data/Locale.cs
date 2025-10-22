using MelonLoader;

using Il2CppAssets.Scripts.Database;
using Il2CppPeroPeroGames.GlobalDefines;

namespace Bnfour.MuseDashMods.SongInfo.Data;

/// <summary>
/// Used to provide localized string to use in mod UI.
/// </summary>
internal static class Locale
{
    // see GlobalDefines.Language, order is
    // 0: dummy "none" entry
    // 1: English
    // 2: Chinese Simplified
    // 3: Chinese Traditional
    // 4: Japanese
    // 5: Korean
    private static readonly string[] _lengthLocalizations = new[]
    {
        "[duration]",
        "Length",
        "[Chinese S]",
        "[Chinese T]",
        "[Japanese]",
        "[Korean]"
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
