using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities
{
    /// <summary>
    /// A class to encapsulate operations with added UI elements.
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

                buttonObj.transform.SetParent(rankCell.transform);
                // for whatever reason, scale for self rank cell is set to 100 100 100 by default
                buttonObj.transform.localScale = new Vector3(1, 1, 1);

                var rect = buttonObj.GetComponent<RectTransform>();
                // hardcoded for 1080 screens for now
                // TODO find a way to work with different resolutions
                rect.sizeDelta = new Vector2(100, 40);
                rect.anchoredPosition3D = new Vector3(200, 0, 0);
            }
        }

        public static void FillData(GameObject rankCell, Data.AdditionalScoreboardDataEntry dataEntry)
        {
            var extraField = rankCell.transform.FindChild(NewComponentId);
            if (extraField != null)
            {
                var textComponent = extraField.transform.GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    textComponent.text = dataEntry.ToString();
                }
                var buttonComponent = extraField.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    // just in case
                    buttonComponent.onClick.RemoveAllListeners();
                    buttonComponent.onClick.AddListener((UnityAction) new Action(() =>
                    {
                        CharacterSwitcher.Switch(dataEntry.CharacterId, dataEntry.ElfinId);
                    }));
                }
            }
        }
    }
}
