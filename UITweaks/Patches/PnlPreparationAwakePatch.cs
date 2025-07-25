using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.Awake))]
public class PnlPreparationAwakePatch
{
    internal static void Postfix(PnlPreparation __instance)
    {
        if (!Melon<UITweaksMod>.Instance.AchievementsHeaderClassicStyling)
        {
            return;
        }

        // TODO null checks

        var iconTransform = __instance.transform.Find("RightRoot/ImgStageAchievement");
        var textTransform = iconTransform.Find("TxtValue");

        var styleSourceText = __instance.transform.Find("Panels/PnlAchv/ImgTittleBaseP/TxtContent")?.GetComponent<Text>();

        var text = textTransform.GetComponent<Text>();
        text.color = styleSourceText.color;
        text.fontSize = styleSourceText.fontSize;
        text.fontStyle = styleSourceText.fontStyle;
        // move the text closer to the icon
        var textRectTransform = textTransform.GetComponent<RectTransform>();
        textRectTransform.anchoredPosition3D += new Vector3(-6, -1, 0);
        // move the entire icon and text group to the right a bit
        var iconRectTransform = iconTransform.GetComponent<RectTransform>();
        iconRectTransform.anchoredPosition3D += new Vector3(53, 0, 0);
    }
}
