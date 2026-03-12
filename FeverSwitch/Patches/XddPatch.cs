using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppUI.Controls.BtnTipStateControl;
using MelonLoader;

namespace Bnfour.MuseDashMods.FeverSwitch.Patches;

/// <summary>
/// 
/// </summary>
[HarmonyPatch(typeof(PnlLevelConfigRandomRoleKeyTipViewControl), nameof(PnlLevelConfigRandomRoleKeyTipViewControl.OnEnable))]
public class XddPatch
{
    internal static void Prefix(out bool __state)
    {
        Melon<FeverSwitchMod>.Logger.Msg("got em");
        // save the current state, whatever it is (false, probably; not that we care)
        __state = DataHelper.isUseRandomLevelConfig;
        // set to match the fever state for the duration of the patched method
        DataHelper.isUseRandomLevelConfig = Melon<FeverSwitchMod>.Instance.IsAutoDefault
            ? !DataHelper.isAutoFever : DataHelper.isAutoFever;
    }

    internal static void Postfix(bool __state)
    {
        DataHelper.isUseRandomLevelConfig = __state;
    }
}
