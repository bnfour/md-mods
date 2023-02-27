using System;
using Assets.Scripts.Database;
using Assets.Scripts.PeroTools.Nice.Components;
using Bnfour.MuseDashMods.ScoreboardCharacters.Data;
using Bnfour.MuseDashMods.ScoreboardCharacters.Extensions;
using MelonLoader;
using UnityEngine;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities
{
    /// <summary>
    /// Handles changes to the selected character and/or elfin.
    /// Also autoscrolls relevant menus.
    /// </summary>
    public class CharacterSwitcher
    {
        private const string PanelSharedPath = "UI/Standerd/PnlMenu/Panels/";

        private FancyScrollView characterScrollView;
        private FancyScrollView elfinScrollView;

        public void Switch(Character character, Elfin elfin)
        {
            var currentCharacter = (Character)DataHelper.selectedRoleIndex;
            var currentElfin = (Elfin)DataHelper.selectedElfinIndex;
            var anyChanges = currentCharacter != character || currentElfin != elfin;

            if (!anyChanges)
            {
                return;
            }

            DataHelper.selectedRoleIndex = (int)character;
            DataHelper.selectedElfinIndex = (int)elfin;

            ScrollMenus(character, elfin);
        }

        public void ResetCache()
        {
            characterScrollView = null;
            elfinScrollView = null;
        }

        private void ScrollMenus(Character character, Elfin elfin)
        {
            // do not scroll if elfin is unequipped
            if (elfin.IsActuallyAnElfin())
            {
                if (!elfin.IsPlaceholderForFuture())
                {
                    elfinScrollView = elfinScrollView ?? GameObject.Find(PanelSharedPath + "PnlElfin")?.GetComponentInChildren<FancyScrollView>();
                    if (elfinScrollView != null)
                    {
                        elfinScrollView.currentScrollPosition = elfin.GetMenuOrder();
                    }
                }
                else
                {
                    var logger = Melon<ScoreboardCharactersMod>.Logger;
                    logger.Warning($"Unknown order for elfin {(int)elfin}, unable to scroll to");
                }
            }
            if (!character.IsPlaceholderForFuture())
            {
                characterScrollView = characterScrollView ?? GameObject.Find(PanelSharedPath + "PnlRole")?.GetComponentInChildren<FancyScrollView>();
                if (characterScrollView != null)
                {
                    characterScrollView.currentScrollPosition = character.GetMenuOrder();
                }
            }
            else
            {
                var logger = Melon<ScoreboardCharactersMod>.Logger;
                logger.Warning($"Unknown order for character {(int)character}, unable to scroll to");
            }
        }
    }
}
