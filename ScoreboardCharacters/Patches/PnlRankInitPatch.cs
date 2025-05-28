using HarmonyLib;
using UnityEngine;

using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine.UI;
using MelonLoader;

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

        var newImageRectTransform = newImage.GetComponent<RectTransform>();
        // copy all the anchor-related stuff from another rect transform that always had
        // our new parent as parent so that this transform is moved similarly
        // TODO this may work for song info's achievements header image probably?
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

        btnCharacterRectTransform.anchoredPosition3D = new Vector3(100, -50, 0);
        // i still have no idea how anchoring works, this kinda works
        newImageRectTransform.anchoredPosition3D = new Vector3(124, -50, 0);
        btnElfinRectTransform.anchoredPosition3D = new Vector3(240, -50, 0);
        btnResetRectTransform.anchoredPosition3D = new Vector3(300, -50, 0);

        // move the "fixed" rootBtnLevelConfigTop to the top bar,
        // at least no hierarchy move is required, yay
        var topRectTransform = topLevelConfigTransform.gameObject.GetComponent<RectTransform>();
        topRectTransform.anchoredPosition3D = new Vector3
        {
            x = topRectTransform.anchoredPosition3D.x + 315,
            y = topRectTransform.anchoredPosition3D.y + 74,
            z = topRectTransform.anchoredPosition3D.z
        };

        // finally, open the extended scoreboard as soon as the screen is seen
        __instance.DoLevelConfigForward();
    }
}
