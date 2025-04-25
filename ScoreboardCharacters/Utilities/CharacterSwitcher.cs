using UnityEngine;

using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using Il2CppAssets.Scripts.PeroTools.Nice.Components;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;
using Il2CppAssets.Scripts.UI.Panels;
using HarmonyLib;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

/// <summary>
/// Handles changes to the selected character and/or elfin.
/// Also autoscrolls relevant menus.
/// </summary>
public class CharacterSwitcher
{
    private const string PanelSharedPath = "UI/Standerd/PnlMenu/Panels/";

    private FancyScrollView _characterScrollView;
    private FancyScrollView _elfinScrollView;

    // TODO find a better way to get the panel instance, like search for it like the scrollviews here
    public void Switch(Character character, Elfin elfin, PnlRank panelToRefresh)
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

        if (panelToRefresh != null)
        {
            Traverse.Create(panelToRefresh).Method("RefreshLevelConfigUi").GetValue();
        }
    }

    public void ResetCache()
    {
        _characterScrollView = null;
        _elfinScrollView = null;
    }

    private void ScrollMenus(Character character, Elfin elfin)
    {
        // one thing to note:
        // DBConfig<N>.Get<N>InfoByIndex returns 1-based order,
        // FancyScrollView uses 0-based order

        var elfinOrder = Singleton<ConfigManager>.instance.GetConfigObject<DBConfigElfin>(-1).GetElfinInfoByIndex((int)elfin)?.order;
        if (elfinOrder != null)
        {
            _elfinScrollView ??= GameObject.Find(PanelSharedPath + "PnlElfin")?.GetComponentInChildren<FancyScrollView>();
            if (_elfinScrollView != null)
            {
                _elfinScrollView.currentScrollPosition = elfinOrder.Value - 1;
            }
        }

        var characterOrder = Singleton<ConfigManager>.instance.GetConfigObject<DBConfigCharacter>(-1).GetCharacterInfoByIndex((int)character)?.order;
        if (characterOrder != null)
        {
            _characterScrollView ??= GameObject.Find(PanelSharedPath + "PnlRole")?.GetComponentInChildren<FancyScrollView>();
            if (_characterScrollView != null)
            {
                _characterScrollView.currentScrollPosition = characterOrder.Value - 1;
            }
        }
    }
}
