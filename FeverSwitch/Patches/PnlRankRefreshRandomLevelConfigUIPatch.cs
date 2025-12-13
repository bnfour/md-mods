using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;

namespace Bnfour.MuseDashMods.FeverSwitch.Patches;

/// <summary>
/// Sets the repurposed toggle to match the fever state on UI refresh.
/// Uses the random mode flag to achieve that.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.RefreshRandomLevelConfigUI))]
public class PnlRankRefreshRandomLevelConfigUIPatch
{
    // tl;dr: random mode itself is not available due to technical reasons

    // the patched method queries the random mode state internally
    // and since this is an Il2Cpp game we can't transpile,
    // we manipulate this state value to set the toggle to the position we want
    // and disabling the random mode for good as soon as the UI is updated,
    // so it is not available at all if this mod is installed

    // TODO check for and disable random mode on startup?
    // it'll probably get disabled by this anyway as soon as the preparation panel is opened

    internal static void Prefix()
    {
        // if auto is default, we negate the current isAutoFever value
        //     auto fever => toggle off => isUseRandomLevelConfig = false
        // if manual is default, we don't negate
        //     auto fever => toggle on => isUseRandomLevelConfig = true
        // extrapolation to manual fever for both defaults is left as an exercise to the reader
        DataHelper.isUseRandomLevelConfig = Melon<FeverSwitchMod>.Instance.IsAutoDefault
            ? !DataHelper.isAutoFever : DataHelper.isAutoFever;
    }

    internal static void Postfix()
    {
        DataHelper.isUseRandomLevelConfig = false;
    }
}
