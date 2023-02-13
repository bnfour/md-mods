using System;
using Assets.Scripts.Database;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Components;
using Bnfour.MuseDashMods.ScoreboardCharacters.Data;
using Bnfour.MuseDashMods.ScoreboardCharacters.Extensions;
using UnityEngine;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities
{
    public class CharacterSwitcher
    {
        // TODO make this switchable externally
        private bool ScrollOnSwitchEnabled = false;
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

            if (ScrollOnSwitchEnabled)
            {
                // do not scroll if elfin is unequipped or we don't know where to scroll
                // TODO nag about update on mystery IDs?
                if (elfin.IsActuallyAnElfin() && !elfin.IsMystery())
                {
                    elfinScrollView = elfinScrollView ?? GameObject.Find(PanelSharedPath + "PnlElfin")?.GetComponentInChildren<FancyScrollView>();
                    if (elfinScrollView != null)
                    {
                        elfinScrollView.currentScrollPosition = elfin.GetMenuOrder();
                    }
                }
                if (!character.IsMystery())
                {
                    characterScrollView = characterScrollView ?? GameObject.Find(PanelSharedPath + "PnlRole")?.GetComponentInChildren<FancyScrollView>();
                    if (characterScrollView != null)
                    {
                        characterScrollView.currentScrollPosition = character.GetMenuOrder();
                    }
                }
            }
        }

        public void ResetCache()
        {
            characterScrollView = null;
            elfinScrollView = null;
        }
    }
}
