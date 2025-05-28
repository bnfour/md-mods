using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

using Il2CppAssets.Scripts.UI.Panels;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

/// <summary>
/// A patch that alters the scoreboard UI
/// and switches to the scoreboard display when the panel is first shown.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.InitLevelConfigBtn))]
// i really wanted it to be an Init patch
// but whenever i try to patch that, the patch does not fire :(
public class PnlRankInitPatch
{
    // TODO split this up somehow? move to UI patcher
    internal static void Postfix(PnlRank __instance)
    {
        // the following wall of text modifies the vanilla UI to make the
        // 5.3.0 character switcher panel on song info screen more compact when
        // minimized

        var topLevelConfigTransform = __instance.transform.Find("Mask/rootBtnLevelConfigTop");

        // set the character switch panel to roll up completely 
        __instance.targetShinkHeight = 0f;

        // hide some components from the view, but not remove them to avoid other code breaking
        // everybody stand back, i know regular^W lambda expressions
        new List<(string componentPath, bool hideImage)>()
        {
            ("ButtonReset/TxtReset", false),
            ("ButtonCha", true),
            ("ButtonElfin", true)
        }.ForEach(tuple => BanishComponent(topLevelConfigTransform, tuple.componentPath, tuple.hideImage));

        // resize and recolor moved buttons to match the top bar's original icons and text
        var originalColor = __instance.transform
            .Find("Mask/ImgBaseDarkP/ImgTittleBaseP/TxtContent")
            .GetComponent<Text>().color;
        // this is just "fancy" way to use an (inlined) function 4 times
        // for 4 different arguments
        // TODO why am i doing this
        new List<string>()
        {
            "ButtonReset/Image",
            "ButtonReset",
            "ButtonCha/Image",
            "ButtonElfin/Image"
        }.ForEach(s =>
        {
            var transform = topLevelConfigTransform.Find(s);
            transform.GetComponent<RectTransform>().sizeDelta = new(40, 40);
            transform.GetComponent<Image>().color = originalColor;
        });

        // clone an image to be the new character/elfin indicator
        var someImage = topLevelConfigTransform.Find("ButtonReset/Image").gameObject;
        var newImage = GameObject.Instantiate(someImage, topLevelConfigTransform);
        // TODO move to constant available to both new patches
        newImage.name = "BnTopLevelConfigState";
        newImage.GetComponent<Image>().color = Color.white;
        var newImageRectTransform = newImage.GetComponent<RectTransform>();
        // copy all the anchor-related stuff from another rect transform that always had
        // our new parent as parent so that this transform is moved similarly
        // i have no idea what is required, so just copy everything to be sure >_<
        // TODO this may work for song info's achievements header image??
        var btnCharacterRectTransform = topLevelConfigTransform.Find("ButtonCha").GetComponent<RectTransform>();
        newImageRectTransform.anchorMin = btnCharacterRectTransform.anchorMin;
        newImageRectTransform.anchorMax = btnCharacterRectTransform.anchorMax;
        newImageRectTransform.pivot = btnCharacterRectTransform.pivot;
        newImageRectTransform.offsetMin = btnCharacterRectTransform.offsetMin;
        newImageRectTransform.offsetMax = btnCharacterRectTransform.offsetMax;
        // resize goes after anchor shenanigans
        newImageRectTransform.sizeDelta = new Vector2(80, 40);

        // move the collapsed level config items within the rootBtnLevelConfigTop to form a line
        // at least there are no repeating long invocations anymore
        // TODO but at what cost?
        new List<(string name, Vector3 position)>
        {
            ("ButtonCha", new Vector3(100, -50, 0)),
            ("BnTopLevelConfigState", new Vector3(124, -50, 0)),
            ("ButtonElfin", new Vector3(240, -50, 0)),
            ("ButtonReset", new Vector3(300, -50, 0))
        }.ForEach(tuple => topLevelConfigTransform.Find(tuple.name).GetComponent<RectTransform>().anchoredPosition3D = tuple.position);

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

    private static void BanishComponent(Transform root, string searchPath, bool hideImage)
    {
        var transform = root.Find(searchPath);
        // hide this instead of original transform if present;
        // present for both elfin and character texts,
        // not present for the random button -- called differently
        // and we're also not removing the non-key legend from it
        var subComponentTrasform = transform.Find("ImgSongTitleMask");
        // to the shadow realm with you
        (subComponentTrasform ?? transform).position = new(99_999f, 99_999f, 0);
        if (hideImage)
        {
            transform.GetComponent<Image>().color = Color.clear;
        }
    }


}
