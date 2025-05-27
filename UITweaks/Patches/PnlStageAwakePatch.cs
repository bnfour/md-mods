using HarmonyLib;
using MelonLoader;
using UnityEngine;

using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Makes the album title text on selection screen wide enough to fit all album
/// titles.
/// </summary>
[HarmonyPatch(typeof(PnlStage), nameof(PnlStage.Awake))]
public class PnlStageAwakePatch
{
    private static void Postfix(PnlStage __instance)
    {
        if (!Melon<UITweaksMod>.Instance.WiderAlbumTitlesEnabled)
        {
            return;
        }

        // resize both the object and the text, it will be centered automatically
        var objectTransform = __instance.m_AlbumTitleObj.GetComponent<RectTransform>();
        var textTransform = __instance.m_AlbumTitleTxt.GetComponent<RectTransform>();

        // new width found empirically, fits all currently (4.11) available titles,
        // the widest being ba's pack full title
        objectTransform.sizeDelta = new Vector2(550, 60);
        textTransform.sizeDelta = new Vector2(550, 60);

        // makes the component aware of its new extra width
        // originally set to sizeDelta's width - 86
        // set to cover slightly more because we don't expect any scrolling to happen
        __instance.m_AlbumTitleTxt.m_LongBound = 480;

        var logger = Melon<UITweaksMod>.Logger;

        logger.Msg($"root name {__instance.m_AlbumTitleObj.name}");

        var maskL = __instance.m_AlbumTitleObj.transform.Find("DisplayArea/ImgMaskL");
        var maskR = __instance.m_AlbumTitleObj.transform.Find("DisplayArea/ImgMaskR");

        var cs = maskL.GetComponents<Component>();
        foreach (var c in cs)
        {
            logger.Msg($"{c.name} -- {c.GetIl2CppType().Name}");
        }

        // maskL.GetComponent<Image>().color = Color.clear;
        // maskR.GetComponent<Image>().color = Color.clear;

        var rt = maskL.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2
        {
            x = rt.sizeDelta.x / 2,
            y = rt.sizeDelta.y
        };

        var rtR = maskR.GetComponent<RectTransform>();
        var originalRight = rtR.right;
        rtR.sizeDelta = new Vector2
        {
            x = rtR.sizeDelta.x / 2,
            y = rtR.sizeDelta.y
        };
        rtR.right = originalRight;

        // logger.Msg($"l = null {maskL == null}, r = null {maskR == null}");
    }
}
