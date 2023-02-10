using System;
using MelonLoader;
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

                // default button also contains a text component, we're not using it
                var text = buttonObj.GetComponentInChildren<Text>();
                text.gameObject.transform.parent = null;

                buttonObj.transform.SetParent(rankCell.transform);
                // for whatever reason, scale for self rank cell is set to 100 100 100 by default
                buttonObj.transform.localScale = new Vector3(1, 1, 1);

                var rect = buttonObj.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(80, 40);
                // TODO adjust as the button got narrower
                rect.anchoredPosition3D = new Vector3(200, 0, 0);
            }
        }

        public static void FillData(GameObject rankCell, Data.AdditionalScoreboardDataEntry dataEntry)
        {
            var extraField = rankCell.transform.FindChild(NewComponentId);
            if (extraField != null)
            {
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
                var imageComponent = extraField.GetComponentInChildren<Image>();
                if (imageComponent != null)
                {
                    var provider = Melon<ScoreboardCharactersMod>.Instance.ButtonImageProvider;
                    imageComponent.sprite = provider.GetSprite(dataEntry.CharacterId, dataEntry.ElfinId);
                }
            }
        }
    }
}
