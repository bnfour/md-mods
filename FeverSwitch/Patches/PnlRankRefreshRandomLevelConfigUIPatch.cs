using HarmonyLib;

using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;

namespace Bnfour.MuseDashMods.FeverSwitch.Patches;

/// <summary>
/// Sets the repurposed toggle to match the fever state on UI refresh.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.RefreshRandomLevelConfigUI))]
public class PnlRankRefreshRandomLevelConfigUIPatch
{
    // tl;dr: random mode itself is not available due to technical reasons

    // the patched method queries the random mode state internally
    // and since this is an Il2Cpp game we can't transpile,
    // we manipulate this state value to set the toggle to the position we want
    // and disabling the random mode for good as soon as the UI is updated

    // TODO check for and disable random mode on startup?
    // it'll probably get disabled by this anyway as soon as the preparation panel is opened

    internal static void Prefix()
    {
        // switch on, which is random on originally, is treated as manual fever
        // TODO an option to reverse, as in manual mode is off/default instead?
        DataHelper.isUseRandomLevelConfig = !DataHelper.isAutoFever;
    }
    
    internal static void Postfix()
    {
        DataHelper.isUseRandomLevelConfig = false;
    }
}
