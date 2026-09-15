using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

[HarmonyPatch(typeof(DBConfigLocalAlbums), nameof(DBConfigLocalAlbums.GetLocalTitleByIndex))]
public class GrooveAlbumTitleFix
{
    internal static void Postfix(ref string __result)
    {
        // TODO do nothing if turned off via config

        // index is not static, as new albums are added to the start of the list,
        // so we can't use a constant number to refer to the album
        // (72 for 6.6.0, would likely increase by 1 for each new album added)
        // note: the extra space; fullwidth exclamation mark
        //                   V        V
        if (__result == "Let' s GROOVE！")
        {
            __result = "Let's GROOVE!";
        }
    }
}
