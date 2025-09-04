using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.PeroTools.GeneralLocalization;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.RankPreview.Patches;

[HarmonyPatch(typeof(PnlVictory), nameof(PnlVictory.InitControl))]
public class TestPatch
{
    internal static void Postfix(PnlVictory __instance)
    {
        var logger = Melon<RankPreviewMod>.Logger;
        logger.Msg($"okayeg eg in {__instance.m_CurControls.GetIl2CppType().ToString()}");

        logger.Msg($"__instance.m_CurControls.scoreTxt.name is {__instance.m_CurControls.scoreTxt.name}");
        logger.Msg($"its parent name is {__instance.m_CurControls.scoreTxt.transform.parent.name}");

        var parent = __instance.m_CurControls.scoreTxt.transform.parent;

        var clone = GameObject.Instantiate(parent, parent.transform.parent);
        clone.name = "FORSNE";
        clone.gameObject.name = "FORSNE";
        GameObject.DestroyImmediate(clone.gameObject.GetComponent<Localization>());
        while (clone.GetChildCount() > 0)
        {
            GameObject.DestroyImmediate(clone.GetChild(0).gameObject);
        }

        clone.GetComponent<Text>().text = "FORSNE";
        clone.GetComponent<RectTransform>().position += new Vector3(0, 10, 0);

        logger.Msg("done?");
    }
}
