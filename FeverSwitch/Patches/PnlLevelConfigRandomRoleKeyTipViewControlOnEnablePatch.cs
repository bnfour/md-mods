using HarmonyLib;

using Il2CppAssets.Scripts.Database;
using Il2CppUI.Controls.BtnTipStateControl;

using Bnfour.MuseDashMods.FeverSwitch.Utilities;

namespace Bnfour.MuseDashMods.FeverSwitch.Patches;

/// <summary>
/// Sets the "random mode" flag to match current fever state for visual consistency
/// for key tip component init (its color depends on the value).
/// See #35.
/// </summary>
[HarmonyPatch(typeof(PnlLevelConfigRandomRoleKeyTipViewControl), nameof(PnlLevelConfigRandomRoleKeyTipViewControl.OnEnable))]
public class PnlLevelConfigRandomRoleKeyTipViewControlOnEnablePatch
{
    internal static void Prefix(out bool __state)
    {
        // save the current state, whatever it is (false, probably; not that we care)
        // to restore after we're done enabling this
        __state = DataHelper.isUseRandomLevelConfig;
        // set to match the fever state for the duration of the patched method
        DataHelper.isUseRandomLevelConfig = FlagOverrideValueProvider.Override;
    }

    internal static void Postfix(bool __state)
    {
        DataHelper.isUseRandomLevelConfig = __state;
    }
}
