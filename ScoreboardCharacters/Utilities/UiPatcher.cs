using System;

using MelonLoader;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

/// <summary>
/// Creates extra UI at runtime, sets click handlers.
/// </summary>
// static to be called in patches
public static class UiPatcher
{
    private const string NewScoreboardComponentName = "BnScoreboardCharactersExtra";

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
    private static Vector3 PixelPerfectishOffsetCorrection(GameObject _)
    {
        // currently the same value for scoreboard entries and self rank;
        // GameObject argument is kept as a discard in case it'll be different again in the future
        // (see history to learn how to discern by name)
        return new(0.5f, -0.08f, 0);
    }
}
