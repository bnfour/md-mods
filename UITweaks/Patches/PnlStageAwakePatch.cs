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
        objectTransform.sizeDelta = new Vector2(470, 60);
        textTransform.sizeDelta = new Vector2(470, 60);

        // makes the component aware of its new extra width
        // originally set to sizeDelta's width - 86
        // set to cover slightly more because we don't expect any scrolling to happen
        // and shrink the side gradients
        __instance.m_AlbumTitleTxt.m_LongBound = 400;

        // shrink the gradients on the sides somewhat so they don't cover even the longest title
        var leftMaskTransform = __instance.m_AlbumTitleObj.transform
            .Find("DisplayArea/ImgMaskL")
            .GetComponent<RectTransform>();
        leftMaskTransform.sizeDelta = new Vector2
        {
            x = leftMaskTransform.sizeDelta.x / 2,
            y = leftMaskTransform.sizeDelta.y
        };

        var rightMaskTransform = __instance.m_AlbumTitleObj.transform
            .Find("DisplayArea/ImgMaskR")
            .GetComponent<RectTransform>();
        // the right image should also be moved after resizing
        var originalRight = rightMaskTransform.right;
        rightMaskTransform.sizeDelta = new Vector2
        {
            x = rightMaskTransform.sizeDelta.x / 2,
            y = rightMaskTransform.sizeDelta.y
        };
        rightMaskTransform.right = originalRight;
    }
}
