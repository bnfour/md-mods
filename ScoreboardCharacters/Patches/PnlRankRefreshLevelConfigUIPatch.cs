using HarmonyLib;

using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

/// <summary>
/// Patch that updates the level config UI custom image whenever the component itself is updated.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.RefreshLevelConfigUI))]
public class PnlRankRefreshLevelConfigUIPatch
{
    internal static void Postfix()
    {
        UiPatcher.UpdateLevelConfigUI();
    }
}
