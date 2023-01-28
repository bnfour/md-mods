using System;
using Assets.Scripts.Database;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities
{
    public static class CharacterSwitcher
    {
        public static void Switch(string characterId, string elfinId)
        {
            // TODO no elfin case

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

            // TODO consider (toggleable?) scrolling to the newly-switched characters in the UI
            // need to update PnlRole.m_CurCellIndex and PnlElfin.m_CurCellIndex
            // note: character order in PnlRole does not match id order,
            // see AdditionalScoreboardDataEntry.GetCharacterMenuOrder
        }
    }
}
