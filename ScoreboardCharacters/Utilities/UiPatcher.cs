using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Il2CppAssets.Scripts.UI.Panels;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

/// <summary>
/// Creates extra UI at runtime, sets click handlers.
/// </summary>
// static to be called in patches
public static class UiPatcher
{
    private const string NewScoreboardComponentName = "BnScoreboardCharactersExtra";
    // this is also used in a patch for searching
    public const string NewConfigUiComponentName = "BnTopLevelConfigState";

    #region scoreboard ui modification

    public static void CreateModUi(GameObject rankCell)
    {
        var addedComponent = rankCell.transform.FindChild(NewScoreboardComponentName);
        if (addedComponent == null)
        {
            var buttonObj = DefaultControls.CreateButton(new DefaultControls.Resources());
            buttonObj.name = NewScoreboardComponentName;

            // default button also contains a text component, we're not using it
            var text = buttonObj.GetComponentInChildren<Text>();
            text.transform.parent = null;

            buttonObj.transform.SetParent(rankCell.transform);
            // for whatever reason, scale for self rank cell is set to 100 100 100 by default
            buttonObj.transform.localScale = new Vector3(1, 1, 1);

            var rect = buttonObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(80, 40);
            rect.anchoredPosition3D = new Vector3(200, 0, 0);
        }
    }

    public static void FillData(GameObject rankCell, Data.AdditionalScoreboardDataEntry dataEntry)
    {
        var extraField = rankCell.transform.FindChild(NewScoreboardComponentName);
        if (extraField != null)
        {
            var buttonComponent = extraField.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.RemoveAllListeners();
                buttonComponent.onClick.AddListener((UnityAction)new Action(() =>
                {
                    var switcher = Melon<ScoreboardCharactersMod>.Instance.CharacterSwitcher;
                    switcher.Switch(dataEntry.Character, dataEntry.Elfin);
                }));

                var imageComponent = buttonComponent.image;
                if (imageComponent != null)
                {
                    var provider = Melon<ScoreboardCharactersMod>.Instance.ButtonImageProvider;
                    imageComponent.sprite = provider.GetSprite(dataEntry.Character, dataEntry.Elfin);
                }
            }
        }
    }

    #endregion
    // don't "quality" open inside
    #region config ui modification (smaller collapsed character switch panel)

    // the following wall of text modifies the vanilla UI to make
    // the 5.3.0 character switcher panel on song info screen more compact when minimized
    // all sizes/offsets found empirically

    public static void MinifyTopLevelConfigUi(PnlRank panel)
    {
        var topLevelConfigTransform = panel.transform.Find("Mask/rootBtnLevelConfigTop");

        // set the character switch panel to roll up completely 
        panel.targetShinkHeight = 0f;

        // hide some components from the view, but not remove them to avoid other code breaking
        // everybody stand back, i know regular^W lambda expressions
        new List<(string componentPath, bool hideImage)>()
        {
            ("ButtonReset/TxtReset", false),
            ("ButtonCha", true),
            ("ButtonElfin", true)
        }.ForEach(tuple => BanishComponent(topLevelConfigTransform, tuple.componentPath, tuple.hideImage));

        // resize and recolor moved buttons to match the top bar's original icons and text
        var originalColor = panel.transform
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
        newImage.name = NewConfigUiComponentName;
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
        var viewportRectTransform = panel.transform.Find("Mask/ImgBaseDarkP/ImgTittleBaseP/ScvRank/Viewport")
            .GetComponent<RectTransform>();
        viewportRectTransform.sizeDelta = new Vector2
        (
            viewportRectTransform.sizeDelta.x,
            viewportRectTransform.sizeDelta.y + 39
        );
        // its background, which technically belongs to the tips panel
        // (the "lost contact with headquarters *kaomoji facepalm*" one)
        // took me way too long to find
        var bgRectTransform = panel.transform.Find("Mask/ImgBaseDarkP/ImgTittleBaseP/ImgRankTips")
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

    #endregion

}
