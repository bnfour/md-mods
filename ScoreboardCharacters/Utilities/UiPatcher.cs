using System;
using Il2CppAssets.Scripts.UI.Panels;
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
    private const string NewComponentId = "BnScoreboardCharactersExtra";

    public static void CreateModUi(GameObject rankCell)
    {
        var addedComponent = rankCell.transform.FindChild(NewComponentId);
        if (addedComponent == null)
        {
            var buttonObj = DefaultControls.CreateButton(new DefaultControls.Resources());
            buttonObj.name = NewComponentId;

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

    public static void FillData(GameObject rankCell, Data.AdditionalScoreboardDataEntry dataEntry,
        // TODO looks kinda scuffed passing a reference through here to CharacterSwitcher
        PnlRank panelToRefresh)
    {
        var extraField = rankCell.transform.FindChild(NewComponentId);
        if (extraField != null)
        {
            var buttonComponent = extraField.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.RemoveAllListeners();
                buttonComponent.onClick.AddListener((UnityAction) new Action(() =>
                {
                    var switcher = Melon<ScoreboardCharactersMod>.Instance.CharacterSwitcher;
                    switcher.Switch(dataEntry.Character, dataEntry.Elfin, panelToRefresh);
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
}
