using HarmonyLib;
using UnityEngine;

using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

/// <summary>
/// A patch that switches to the scoreboard display when the panel is first shown.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.InitLevelConfigBtn))]
// i really wanted it to be an Init patch
// but whenever i try to patch that, the patch does not fire :(
public class PnlRankInitPatch
{
    internal static void Postfix(PnlRank __instance)
    {
        // the following wall of text modifies the vanilla UI to make the
        // 5.3.0 character switcher panel on song info screen more compact when
        // minimized

        var topLevelConfigTransform = __instance.transform.Find("Mask/rootBtnLevelConfigTop");

        // set the character switch panel to roll up completely 
        __instance.targetShinkHeight = 0f;

        // hide some components from the view, but not remove them to avoid other code breaking
        Vector3 shadowRealm = new(99_999f, 99_999f, 0f);
        // just the text for reset button
        topLevelConfigTransform.Find("ButtonReset/TxtReset")
            .position = shadowRealm;
        // text and non-key icon for character and elfin buttons:
        // moving the icon for some reason moves both icons;
        // fortunately, we can just set their color to transparent
        var btnCharacter = topLevelConfigTransform.Find("ButtonCha");
        btnCharacter.Find("ImgSongTitleMask").position = shadowRealm;
        btnCharacter.GetComponent<Image>().color = Color.clear;
        var btnElfin = topLevelConfigTransform.Find("ButtonElfin");
        btnElfin.Find("ImgSongTitleMask").position = shadowRealm;
        btnElfin.GetComponent<Image>().color = Color.clear;

        // TODO button image sizes and color to match ones originally from the bar

        // clone an image to be the new character/elfin indicator
        var someImage = topLevelConfigTransform.Find("ButtonReset/Image").gameObject;
        var newImage = GameObject.Instantiate(someImage, topLevelConfigTransform);
        newImage.name = "BnTopLevelConfigState";
        newImage.GetComponent<Image>().color = Color.white;
        // copy all the anchor-related stuff from another rect transform that always had
        // our new parent as parent so that this transform is moved similarly
        // TODO this may work for song info's achievements header image probably?
        var newImageRectTransform = newImage.GetComponent<RectTransform>();

        var btnCharacterRectTransform = btnCharacter.GetComponent<RectTransform>();
        newImageRectTransform.anchorMin = btnCharacterRectTransform.anchorMin;
        newImageRectTransform.anchorMax = btnCharacterRectTransform.anchorMax;
        newImageRectTransform.pivot = btnCharacterRectTransform.pivot;
        newImageRectTransform.offsetMin = btnCharacterRectTransform.offsetMin;
        newImageRectTransform.offsetMax = btnCharacterRectTransform.offsetMax;
        // resize goes after anchor shenanigans
        newImageRectTransform.sizeDelta = new Vector2(80, 40);

        // move the collapsed level config items within the rootBtnLevelConfigTop
        // to form a line
        var btnElfinRectTransform = btnElfin.GetComponent<RectTransform>();
        var btnResetRectTransform = topLevelConfigTransform.Find("ButtonReset").gameObject.GetComponent<RectTransform>();
        // TODO finetune the positions
        btnCharacterRectTransform.anchoredPosition3D = new Vector3(100, -50, 0);
        // i still have no idea how anchoring works, this kinda works
        newImageRectTransform.anchoredPosition3D = new Vector3(124, -50, 0);
        btnElfinRectTransform.anchoredPosition3D = new Vector3(240, -50, 0);
        btnResetRectTransform.anchoredPosition3D = new Vector3(300, -50, 0);

        // move the "fixed" rootBtnLevelConfigTop to the top bar,
        // at least no hierarchy move is required, yay
        var topRectTransform = topLevelConfigTransform.gameObject.GetComponent<RectTransform>();
        topRectTransform.anchoredPosition3D = new Vector3
        (
            topRectTransform.anchoredPosition3D.x + 315,
            topRectTransform.anchoredPosition3D.y + 74,
            topRectTransform.anchoredPosition3D.z
        );

        // edit the expanded scoreboard to fill the empty space created from
        // minimizing the switcher completely (still less space than OG though,
        // fits ~7.5 scoreboard entries as compared to original full 8)

        // the scoreboard itself
        var viewportRectTransform = __instance.transform.Find("Mask/ImgBaseDarkP/ImgTittleBaseP/ScvRank/Viewport")
            .GetComponent<RectTransform>();
        viewportRectTransform.sizeDelta = new Vector2
        (
            viewportRectTransform.sizeDelta.x,
            viewportRectTransform.sizeDelta.y + 39
        );
        // its background, which technically belongs to the tips panel
        // (the "lost contact with headquarters *kaomoji facepalm*" one)
        // took me way too long to find
        var bgRectTransform = __instance.transform.Find("Mask/ImgBaseDarkP/ImgTittleBaseP/ImgRankTips")
            .GetComponent<RectTransform>();
        bgRectTransform.sizeDelta = new Vector2
        (
            bgRectTransform.sizeDelta.x,
            bgRectTransform.sizeDelta.y + 74
        );
        // also move by half added size due to positioning quirks
        bgRectTransform.anchoredPosition3D = new Vector3
        (
            bgRectTransform.anchoredPosition3D.x,
            bgRectTransform.anchoredPosition3D.y - 37,
            bgRectTransform.anchoredPosition3D.z
        );

        // the self rank line is moved in PnlRankRefreshLevelConfigUiPatch
        // because its position changes outside of this code and can't be set once and for all

        // finally, open the extended scoreboard as soon as the screen is seen
        __instance.DoLevelConfigForward();
    }
}
