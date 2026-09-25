using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Applies formatting fixes for level designer strings of 29-0, commonly known
/// as Groove Prayer.
/// </summary>
[HarmonyPatch(typeof(MusicInfo), nameof(MusicInfo.GetLevelDesignerStringByIndex))]
public class GrooveLevelDesignerFix
{
    internal static void Postfix(MusicInfo __instance, ref string __result)
    {
        if (Melon<UITweaksMod>.Instance.FixGrooveCoasterTexts
            // no MusicUidDefine entry as of 6.6.0
            && __instance?.uid == "29-0")
        {
            __result = __result
                ?.Replace("' ", "'")
                ?.Replace(" !", "!");
        }
    }
}
