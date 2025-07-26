using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Patch that modifies the achievement count text and icon:
/// - text style to match the text header nearby
/// - appearance animation to match
/// </summary>
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

        if (iconTransform != null)
        {
            // set the appearance animations like the header these elements are on
            var animation = iconTransform.gameObject.AddComponent<Animation>();
            var clip = new AnimationClip()
            {
                // it seems we can only add legacy animations at runtime;
                // of course, existing animations use the modern system and cannot be extended
                legacy = true,
                name = "BnClassicHeaderAnimationClip"
            };
            // alpha channel animation: 0 to 1 in 1/6 of a second
            var alphaCurve = new AnimationCurve(new(0, 0), new(1f / 6, 1));
            clip.SetCurve("", Il2CppType.Of<CanvasGroup>(), "m_Alpha", alphaCurve);
            // position animation: move 100 units (pixels?) to the left; end up in original position in 1/6 of a second
            var originalPosition = iconTransform.GetComponent<RectTransform>()?.anchoredPosition.x ?? 0f;
            var xCurve = new AnimationCurve(new(0, originalPosition + 100), new(1f / 6, originalPosition));
            clip.SetCurve("", Il2CppType.Of<RectTransform>(), "m_AnchoredPosition.x", xCurve);

            animation.AddClip(clip, clip.name);
            animation.clip = clip;
        }
    }
}
