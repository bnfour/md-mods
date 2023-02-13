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
    public static class CharacterSwitcher
    {
        // TODO make this switchable externally
        private static bool ScrollOnSwitchEnabled = false;
        private const string PanelSharedPath = "UI/Standerd/PnlMenu/Panels/";

        public static void Switch(Character character, Elfin elfin)
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
                    // TODO consider caching the panel references
                    // also this seems to break some animations for the heart background a little (?)
                    var elfinPanel = GameObject.Find(PanelSharedPath + "PnlElfin")?.GetComponentInChildren<FancyScrollView>();
                    if (elfinPanel != null)
                    {
                        elfinPanel.currentScrollPosition = elfin.GetMenuOrder();
                    }
                }
                if (!character.IsMystery())
                {
                    var characterPanel = GameObject.Find(PanelSharedPath + "PnlRole")?.GetComponentInChildren<FancyScrollView>();
                    if (characterPanel != null)
                    {
                        characterPanel.currentScrollPosition = character.GetMenuOrder();
                    }
                }
            }
        }
    }
}
