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
/// Also autoscrolls relevant menus.
/// </summary>
public class CharacterSwitcher
{
    private const string scrollviewsPanelSharedPath = "UI/Standerd/PnlMenu/Panels/";
    private const string pnlRankPath = "UI/Standerd/PnlPreparation/Panels/PnlRankLocalization/Pc/PnlRank";

    private FancyScrollView _characterScrollView;
    private FancyScrollView _elfinScrollView;
    private PnlRank _pnlRank;

    // TODO find a better way to get the panel instance, like search for it like the scrollviews here
    public void Switch(Character character, Elfin elfin)
    {
        var currentCharacter = (Character)DataHelper.selectedRoleIndex;
        var currentElfin = (Elfin)DataHelper.selectedElfinIndex;
        var anyChanges = currentCharacter != character || currentElfin != elfin;

        if (!anyChanges)
        {
            return;
        }
        // TODO these now check for "overriding just for this level" mode
        DataHelper.selectedRoleIndex = (int)character;
        DataHelper.selectedElfinIndex = (int)elfin;

        ScrollMenus(character, elfin);
        UpdateLevelConfigUI();
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
            Traverse.Create(_pnlRank).Method("RefreshLevelConfigUi").GetValue();
        }
    }
}
