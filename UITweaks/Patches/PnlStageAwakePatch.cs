using HarmonyLib;
using MelonLoader;
using UnityEngine;

using Il2CppAssets.Scripts.UI.Panels;

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
    }
}
