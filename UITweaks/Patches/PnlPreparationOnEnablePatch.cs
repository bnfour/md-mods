using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Patch that fires the custom component appearance animation when the panel appears on the screen.
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.OnEnable))]
public class PnlPreparationOnEnablePatch
{
    internal static void Postfix(PnlPreparation __instance)
    {
        if (!Melon<UITweaksMod>.Instance.AchievementsHeaderClassicStyling)
        {
            return;
        }

        var animation = GameObject.FindObjectOfType<PnlPreparation>()?.transform.Find("RightRoot/ImgStageAchievement")?.GetComponent<Animation>();
        animation?.Play(animation.clip?.name);
    }
}
