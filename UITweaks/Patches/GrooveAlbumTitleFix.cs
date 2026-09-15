using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

[HarmonyPatch(typeof(DBConfigLocalAlbums), nameof(DBConfigLocalAlbums.GetLocalTitleByIndex))]
public class GrooveAlbumTitleFix
{
    internal static void Postfix(int index, ref string __result)
    {
        Melon<UITweaksMod>.Logger.Msg($"Album name for id {index} is {__result}");
    }
}
