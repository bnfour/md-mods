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

        var iconTransform = __instance.transform.Find("RightRoot/ImgStageAchievement");
        var textTransform = iconTransform?.Find("TxtValue");

        var styleSourceText = __instance.transform.Find("Panels/PnlAchv/ImgTittleBaseP/TxtContent")?.GetComponent<Text>();
        Melon<UITweaksMod>.Logger.Msg($"src anchor is {styleSourceText.alignment}");
        var text = textTransform?.GetComponent<Text>();

        if (text != null)
        {
            // text's positioning strategy is "constant 20px gap to the right edge of the screen"
            text.alignment = TextAnchor.MiddleRight;
            if (styleSourceText != null)
            {
                text.color = styleSourceText.color;
                text.fontSize = styleSourceText.fontSize;
                text.fontStyle = styleSourceText.fontStyle;
            }
        }

        // move the text closer to the icon
        var textRectTransform = textTransform?.GetComponent<RectTransform>();
        if (textRectTransform != null)
        {
            textRectTransform.anchoredPosition3D += new Vector3(-22, -1, 0);
        }
        // move the entire icon and text group to the right a bit
        var iconRectTransform = iconTransform?.GetComponent<RectTransform>();
        if (iconRectTransform != null)
        {
            iconRectTransform.anchoredPosition3D += new Vector3(53, 0, 0);
        }
    }
}
