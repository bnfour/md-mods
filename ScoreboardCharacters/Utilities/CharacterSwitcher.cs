using HarmonyLib;
using UnityEngine;

using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using Il2CppAssets.Scripts.PeroTools.Nice.Components;
using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

/// <summary>
/// Handles changes to the selected character and/or elfin.
/// Sets the the global character/elfin config and then resets the current level's config to that.
/// Also autoscrolls relevant settings menus.
/// </summary>
public class CharacterSwitcher
{
    private const string scrollviewsPanelSharedPath = "UI/Standerd/PnlMenu/Panels/";
    private const string pnlRankPath = "UI/Standerd/PnlPreparation/Panels/PnlRankLocalization/Pc/PnlRank";

    private FancyScrollView _characterScrollView;
    private FancyScrollView _elfinScrollView;
    private PnlRank _pnlRank;

    public void Switch(Character character, Elfin elfin)
    {
        // TODO post 5.6.0 level config state??


        // can be either custom or default
        // depends on whether the level has a custom config at the moment
        var levelConfigState = GlobalDataBase.s_DbLevelConfig.curLevelConfigState;

        var currentLevelCharacter = (Character)DataHelper.selectedRoleIndex;
        var currentLevelElfin = (Elfin)DataHelper.selectedElfinIndex;

        // the rest of the method operates on the global config,
        // previous value restored by the method after execution
        GlobalDataBase.s_DbLevelConfig.curLevelConfigState = CurLevelConfigState.Default;

        var currentGlobalCharacter = (Character)DataHelper.selectedRoleIndex;
        var currentGlobalElfin = (Elfin)DataHelper.selectedElfinIndex;

        var anyChanges = currentLevelCharacter != character || currentLevelElfin != elfin
            || currentGlobalCharacter != character || currentGlobalElfin != elfin;

        if (!anyChanges)
        {
            GlobalDataBase.s_DbLevelConfig.curLevelConfigState = levelConfigState;
            return;
        }

        DataHelper.selectedRoleIndex = (int)character;
        DataHelper.selectedElfinIndex = (int)elfin;
        // resetting the current level config effectively saves the character/elfin
        // we're setting now, for this level (even if the global config changes later)
        // until they are changed via custom buttons and/or J/K shortcut keys for this specific level
        GlobalDataBase.s_DbLevelConfig.ResetCurLevelConfig();
        UpdateLevelConfigUI();

        ScrollMenus(character, elfin);

        GlobalDataBase.s_DbLevelConfig.curLevelConfigState = levelConfigState;
    }

    public void ResetCache()
    {
        _characterScrollView = null;
        _elfinScrollView = null;

        _pnlRank = null;
    }

    private void ScrollMenus(Character character, Elfin elfin)
    {
        // one thing to note:
        // DBConfig<N>.Get<N>InfoByIndex returns 1-based order,
        // FancyScrollView uses 0-based order

        var elfinOrder = Singleton<ConfigManager>.instance.GetConfigObject<DBConfigElfin>(-1).GetElfinInfoByIndex((int)elfin)?.order;
        if (elfinOrder != null)
        {
            _elfinScrollView ??= GameObject.Find(scrollviewsPanelSharedPath + "PnlElfin")?.GetComponentInChildren<FancyScrollView>();
            if (_elfinScrollView != null)
            {
                _elfinScrollView.currentScrollPosition = elfinOrder.Value - 1;
            }
        }

        var characterOrder = Singleton<ConfigManager>.instance.GetConfigObject<DBConfigCharacter>(-1).GetCharacterInfoByIndex((int)character)?.order;
        if (characterOrder != null)
        {
            _characterScrollView ??= GameObject.Find(scrollviewsPanelSharedPath + "PnlRole")?.GetComponentInChildren<FancyScrollView>();
            if (_characterScrollView != null)
            {
                _characterScrollView.currentScrollPosition = characterOrder.Value - 1;
            }
        }
    }

    private void UpdateLevelConfigUI()
    {
        _pnlRank ??= GameObject.Find(pnlRankPath)?.GetComponent<PnlRank>();
        if (_pnlRank != null)
        {
            // TODO probably update the new character select floating thingy here
        }
    }
}
