using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;
using Il2CppAssets.Scripts.PeroTools.GeneralLocalization;
using Il2CppInterop.Runtime;

using Bnfour.MuseDashMods.RankPreview.Data;

namespace Bnfour.MuseDashMods.RankPreview.Patches;

[HarmonyPatch(typeof(PnlVictory), nameof(PnlVictory.InitControl))]
public class PnlVictoryInitControlPatch
{
    internal static void Postfix(PnlVictory __instance)
    {
        var scoreTitleText = __instance?.m_CurControls?.scoreTxt?.transform?.parent;
        if (scoreTitleText != null)
        {
            var clone = GameObject.Instantiate(scoreTitleText.gameObject, scoreTitleText.transform.parent);

            clone.name = Constants.ExtraComponentName;
            clone.transform.name = Constants.ExtraComponentName;
            // remove the child components (actual score and "new best" texts)
            while (clone.transform.GetChildCount() > 0)
            {
                GameObject.DestroyImmediate(clone.transform.GetChild(0).gameObject);
            }
            // also remove the Localization component to be sure it does not interfere
            // (no idea how it works ¯\_(ツ)_/¯)
            GameObject.DestroyImmediate(clone.GetComponent<Localization>());

            var text = clone.GetComponent<Text>();
            if (text != null)
            {
                text.text = string.Empty;
                text.fontSize = 60;
            }
            var transform = clone.GetComponent<RectTransform>();
            if (transform != null)
            {
                transform.position += new Vector3(-4, 8, 0);
            }
            clone.AddComponent<CanvasGroup>();
            var animation = clone.AddComponent<Animation>();
            AnimationClip clip = new()
            {
                legacy = true,
                name = "BnRankPreviewCustomAnimation"
            };

            const float startDelay = 1.25f;
            const float endTime = startDelay + 1f / 3;

            clip.SetCurve("", Il2CppType.Of<CanvasGroup>(), "m_Alpha",
                new(new(0, 0), new(startDelay, 0), new(endTime, 1)));

            clip.SetCurve("", Il2CppType.Of<RectTransform>(), "m_AnchoredPosition.y",
                new(new(0, 200), new(startDelay, 200), new(endTime, 230)));

            animation.AddClip(clip, clip.name);
            animation.clip = clip;
        }
    }
}
