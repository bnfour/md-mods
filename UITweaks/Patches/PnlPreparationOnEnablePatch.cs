using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Patch that fires the custom component appearance animations when the panel appears on the screen.
/// (Animations are enabled separately.)
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.OnEnable))]
public class PnlPreparationOnEnablePatch
{
    internal static void Postfix(PnlPreparation __instance)
    {
        var modInstance = Melon<UITweaksMod>.Instance;
        // TODO consider moving component names to constants throughout the mod
        if (modInstance.AchievementsHeaderClassicStyling)
        {
            var headerAnimation = __instance?.transform.Find("RightRoot/ImgStageAchievement")?.GetComponent<Animation>();
            headerAnimation?.Play(headerAnimation.clip?.name);
        }
        if (modInstance.AnimateCharacterSelector)
        {
            var selectAnimation = __instance?.transform.Find("RightRoot/Top")?.GetComponent<Animation>();
            selectAnimation?.Play(selectAnimation.clip?.name);
        }
    }
}
