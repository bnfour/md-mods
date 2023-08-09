using Assets.Scripts.Database;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Components;
using Bnfour.MuseDashMods.ScoreboardCharacters.Data;
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

        // TODO actually test once servers are up
        private void ScrollMenus(Character character, Elfin elfin)
        {
            
            var elfinOrder = Singleton<ConfigManager>.instance.GetConfigObject<DBConfigElfin>(-1).GetElfinInfoByIndex((int)elfin)?.order;
            if (elfinOrder != null)
            {
                elfinScrollView = elfinScrollView ?? GameObject.Find(PanelSharedPath + "PnlElfin")?.GetComponentInChildren<FancyScrollView>();
                if (elfinScrollView != null)
                {
                    elfinScrollView.currentScrollPosition = elfinOrder.Value;
                }
            }

            var characterOrder = Singleton<ConfigManager>.instance.GetConfigObject<DBConfigCharacter>(-1).GetCharacterInfoByIndex((int)character)?.order;
            if (characterOrder != null)
            {
                characterScrollView = characterScrollView ?? GameObject.Find(PanelSharedPath + "PnlRole")?.GetComponentInChildren<FancyScrollView>();
                if (characterScrollView != null)
                {
                    characterScrollView.currentScrollPosition = characterOrder.Value;
                }
            }
        }
    }
}
