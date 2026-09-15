using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

[HarmonyPatch(typeof(MusicInfo), nameof(MusicInfo.GetLevelDesignerStringByIndex))]
public class GrooveLevelDesignerFix
{
    internal static void Postfix(int index, MusicInfo __instance, ref string __result)
    {
        Melon<UITweaksMod>.Logger.Msg($"Level designer for {__instance.uid}_{index} is {__result}");
    }
}
