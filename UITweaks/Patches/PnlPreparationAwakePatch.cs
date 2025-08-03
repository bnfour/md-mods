using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Patch that modifies the PnlPreparation UI, if configured to do so:<br/>
/// - text style for the achievements counter to match the text header nearby<br/>
/// - appearance animations for:<br/>
/// - - the aforementioned achievements counter<br/>
/// - - character selector
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.Awake))]
public class PnlPreparationAwakePatch
{
    internal static void Postfix(PnlPreparation __instance)
    {
        var modInstance = Melon<UITweaksMod>.Instance;

        // TODO consider moving UI patching to separate methods
        if (modInstance.AchievementsHeaderClassicStyling)
        {
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

        if (modInstance.AnimateCharacterSelector)
        {
            var characterSelect = __instance.transform.Find("RightRoot/Top");
            if (characterSelect != null)
            {
                characterSelect.gameObject.AddComponent<CanvasGroup>();
                var animation = characterSelect.gameObject.AddComponent<Animation>();
                var clip = new AnimationClip()
                {
                    legacy = true,
                    name = "BnCharacterSelectAnimationClip"
                };
                // alpha channel animation: 0 to 1 in 1/6 of a second
                // TODO consider changing timing, adding fly-in and/or size expand (like the song title)
                var alphaCurve = new AnimationCurve(new(0, 0), new(1f / 6, 1));
                clip.SetCurve("", Il2CppType.Of<CanvasGroup>(), "m_Alpha", alphaCurve);

                animation.AddClip(clip, clip.name);
                animation.clip = clip;
            }
        }
    }
}
