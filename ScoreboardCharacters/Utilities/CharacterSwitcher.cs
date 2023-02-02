using System;
using Assets.Scripts.Database;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Components;
using UnityEngine;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities
{
    public static class CharacterSwitcher
    {
        // TODO make this switchable externally
        private static bool ScrollOnSwitchEnabled = false;
        private const string PanelSharedPath = "UI/Standerd/PnlMenu/Panels/";

        public static void Switch(string characterId, string elfinId)
        {
            var currentCharacterId = DataHelper.selectedRoleIndex;
            var currentElfinId = DataHelper.selectedElfinIndex;
            // here, these are finally used as ints
            var newCharacterId = int.Parse(characterId);
            var newElfinId = int.Parse(elfinId);

            DataHelper.selectedRoleIndex = newCharacterId;
            DataHelper.selectedElfinIndex = newElfinId;
            // play a different sound if already switched to this combination
            var soundToPlay = currentCharacterId != newCharacterId || currentElfinId != newElfinId
                ? "sfx_switch"
                : "sfx_common_back";
            var volume = DataHelper.sfxVolume;
            var audioManager = Singleton<AudioManager>.instance;
            audioManager.PlayOneShot(soundToPlay, volume, null);

            if (ScrollOnSwitchEnabled)
            {
                // do not scroll if elfin is unequipped
                if (newElfinId >= 0)
                {
                    // TODO consider caching the panel references
                    // also this seems to break some animations for the heart background a little (?)
                    var elfinPanel = GameObject.Find(PanelSharedPath + "PnlElfin")?.GetComponentInChildren<FancyScrollView>();
                    if (elfinPanel != null)
                    {
                        elfinPanel.currentScrollPosition = OrderHelper.GetElfinMenuOrder(newElfinId);
                    }
                }

                var characterPanel = GameObject.Find(PanelSharedPath + "PnlRole")?.GetComponentInChildren<FancyScrollView>();
                if (characterPanel != null)
                {
                    characterPanel.currentScrollPosition = OrderHelper.GetCharacterMenuOrder(newCharacterId);
                }
            }
        }
    }
}
