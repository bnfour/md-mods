using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;
using Il2CppAssets.Scripts.PeroTools.GeneralLocalization;

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
            }
            var transform = clone.GetComponent<RectTransform>();
            if (transform != null)
            {
                // TODO adjust the position
                transform.position += new Vector3(0, 10, 0);
            }
            // TODO set up appearance animation: alpha and moving up,
            // similar to "new best"
        }
    }
}
