using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;

namespace Bnfour.MuseDashMods.FeverSwitch.Patches;

/// <summary>
/// Slightly resizes the random toggle image to be a 42×42 square to fit the auto icon.
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.Awake))]
public class PnlPreparationAwakePatch
{
    internal static void Postfix(PnlPreparation __instance)
    {
        if (__instance.transform.Find("RightRoot/Top/ImgRandomBg/ImgRandomFlagBg/ImgRandomFlag")
            ?.GetComponent<Image>()?.rectTransform is RectTransform rt)
        {
            rt.sizeDelta = new Vector2(42, 42);
        }
    }
}
