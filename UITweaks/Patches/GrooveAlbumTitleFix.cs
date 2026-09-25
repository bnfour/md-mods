using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Fixes the formatting for Groove Coaster's collab album title.
/// Applied to all languages; more prominent results for English and Korean due
/// to different fonts used for different languages.
/// </summary>
/// <remarks>
/// While the patched method has an "album index" argument, data for new albums
/// is usually added towards the beginning of the file, and we can't use a constant
/// index to pin the album. E.g. it's 72 as of 6.6.0, but would increase for each
/// new album added to the game, assuming its data would go before existing albums.
/// </remarks>
[HarmonyPatch(typeof(DBConfigLocalAlbums), nameof(DBConfigLocalAlbums.GetLocalTitleByIndex))]
public class GrooveAlbumTitleFix
{
    internal static void Postfix(ref string __result)
    {
        if (Melon<UITweaksMod>.Instance.FixGrooveCoasterTexts
            // note: the extra space; fullwidth exclamation mark
            //                   V        V
            && __result == "Let' s GROOVE！")
        {
            __result = "Let's GROOVE!";
        }
    }
}
