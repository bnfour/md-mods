using System;
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
            rect.anchoredPosition3D = new Vector3(200, 0, 0) + PixelPerfectishOffsetCorrection(rankCell);
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

    // empirically found offsets to snap the sprites to whole-pixel grid close enough to prevent noticeable smudging
    private static Vector3 PixelPerfectishOffsetCorrection(GameObject rankCell)
    {
        return rankCell.name switch
        {
            // scoreboard cell
            "RankCell_4-3" => new(0.5f, 0.11f, 0),
            // self-rank cell
            "PlayerRankCell_4-3" => new(0.5f, -0.44f, 0),
            // should never happen
            // TODO check somewhere?
            _ => new(0, 0, 0)
        };
    }

    #endregion
    // don't "quality" open inside
    #region config ui modification (smaller collapsed character switch panel)

    // the following modifies the vanilla UI to make the 5.3.0 character
    // switcher panel on song info screen more compact when minimized
    // all sizes/offsets found empirically

    // basically, such code should not be written by hand:
    // you might remember *.Designer.cs files from winforms era
    // -- this is pretty much the same UI code that should be done via Unity editor,
    // so in order to fight boredom and walls of similar assignments/calls,
    // i freestyle with lambdas and/or reflection here 
    // (performance overhead _should_ be negligible)
    // you have been warned

    public static void MinifyTopLevelConfigUi(PnlRank panel)
    {
        var topLevelConfigTransform = panel.transform.Find("Mask/rootBtnLevelConfigTop");

        // set the character switch panel to roll up completely 
        panel.targetShinkHeight = 0f;

        // hide some components from the view, but not remove them to avoid other code breaking
        // everybody stand back, i know regular^W lambda expressions
        Array.ForEach(new (string componentPath, bool hideImage)[]
        {
            ("ButtonReset/TxtReset", false),
            ("ButtonCha", true),
            ("ButtonElfin", true),
            ("txtRandomTipTop", false)
        },
        // fun fact: as of now, it's impossible to decostruct the tuple in lambda's definition
        tuple => BanishComponent(topLevelConfigTransform, tuple.componentPath, tuple.hideImage));

        // resize and recolor moved buttons to match the top bar's original icons and text
        var originalColor = panel.transform
            .Find("Mask/ImgBaseDarkP/ImgTittleBaseP/TxtContent")
            .GetComponent<Text>().color;
        Array.ForEach(new[]
        {
            "ButtonReset/Image",
            "ButtonReset",
            "ButtonCha/Image",
            "ButtonElfin/Image"
        }, s =>
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
        var btnCharacterRectTransform = topLevelConfigTransform.Find("ButtonCha").GetComponent<RectTransform>();
        var rectTransformType = typeof(RectTransform);
        Array.ForEach(new[]
        {
            // i have no idea what is required, so just copy everything to be sure >_<
            "anchorMin",
            "anchorMax",
            "pivot",
            "offsetMin",
            "offsetMax"
        }, s =>
        {
            var propertyInfo = rectTransformType.GetProperty(s);
            var originalValue = propertyInfo.GetValue(btnCharacterRectTransform);
            propertyInfo.SetValue(newImageRectTransform, originalValue);
        });

        // resize goes after anchor shenanigans
        newImageRectTransform.sizeDelta = new Vector2(80, 40);

        // move the collapsed level config items within the rootBtnLevelConfigTop to form a line
        Array.ForEach(new (string name, Vector3 position)[]
        {
            // the original position of character button is about (100, -50)
            // move other components near it
            ("ButtonCha", new Vector3(100, -50, 0)),
            ("BnTopLevelConfigState", new Vector3(124, -50, 0)),
            ("ButtonElfin", new Vector3(240, -50, 0)),
            ("ButtonReset", new Vector3(300, -50, 0))
        }, tuple => topLevelConfigTransform.Find(tuple.name).GetComponent<RectTransform>().anchoredPosition3D = tuple.position);

        // move the "fixed" rootBtnLevelConfigTop to the top bar,
        // at least no hierarchy move is required, yay
        Move(topLevelConfigTransform, new(315, 74));

        // edit the expanded scoreboard to fill the empty space created from
        // minimizing the switcher completely (still less space than OG though,
        // fits ~7 scoreboard entries as compared to original 8)

        // the scoreboard itself
        var viewportRect = panel.transform.Find("Mask/ImgBaseDarkP/ImgTittleBaseP/ScvRank/Viewport");
        Resize(viewportRect, new(0, 80));
        Move(viewportRect, new(0, 4));

        // its background, which technically belongs to the tips panel
        // (the "lost contact with headquarters *kaomoji facepalm*" one)
        // took me way too long to find
        var bgRect = panel.transform.Find("Mask/ImgBaseDarkP/ImgTittleBaseP/ImgRankTips");
        Resize(bgRect, new(0, 74));
        // also move by half added size due to positioning quirks
        Move(bgRect, new(0, -37));

        // the self rank line is moved in PnlRankRefreshLevelConfigUiPatch
        // because its position changes outside of this code and can't be set once and for all

        // fix the random button being slightly off
        var randomButton = panel.transform.Find("Mask/BtnRandomReset");
        Move(randomButton, new(0, -2));
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

    // these are relative

    public static void Resize(Transform parent, Vector2 delta)
    {
        var rectTransform = parent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2
        (
            rectTransform.sizeDelta.x + delta.x,
            rectTransform.sizeDelta.y + delta.y
        );
    }

    public static void Move(Transform parent, Vector2 delta)
    {
        var rectTransform = parent.GetComponent<RectTransform>();
        rectTransform.anchoredPosition3D = new Vector3
        (
            rectTransform.anchoredPosition3D.x + delta.x,
            rectTransform.anchoredPosition3D.y + delta.y,
            rectTransform.anchoredPosition3D.z
        );
    }

    #endregion

}
