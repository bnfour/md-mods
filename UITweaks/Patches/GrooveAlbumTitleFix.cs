using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

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
