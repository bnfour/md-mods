using MelonLoader;

using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.FeverSwitch.Utilities;

/// <summary>
/// Holds the logic to get the override value for DataHelper.isUseRandomLevelConfig
/// from current fever state and mod config. The override is used to trick the UI
/// elements into displaying the fever state, even though they are hardcoded to
/// read the random mode state.
/// </summary>
internal static class FlagOverrideValueProvider
{
    // if auto is default, we negate the current isAutoFever value
    //     auto fever => toggle off => isUseRandomLevelConfig = false
    // if manual is default, we don't negate
    //     auto fever => toggle on => isUseRandomLevelConfig = true
    // extrapolation to manual fever for both defaults is left as an exercise to the reader
    internal static bool Override => Melon<FeverSwitchMod>.Instance.IsAutoDefault
        ? !DataHelper.isAutoFever
        : DataHelper.isAutoFever;

}
