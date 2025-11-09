using HarmonyLib;

using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;

namespace Bnfour.MuseDashMods.FeverSwitch.Patches;

/// <summary>
/// Handles the random mode switch activation,
/// switching the fever mode instead.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.OnClickRandom))]
public class PnlRankOnClickRandomPatch
{
    internal static bool Prefix(PnlRank __instance)
    {
        // change the actual flag value
        DataHelper.isAutoFever = !DataHelper.isAutoFever;

        // let the settings panel know (if it exists): it does not expect anything else can change this value,
        // so it's usually read on init only
        var inputPanel = PnlInputPc.Instance();
        // it is possible that Instance returns a non-null panel which is still not initialized
        // and will throw inside SetIsManualFever, so there is an additional check
        if (inputPanel != null && inputPanel.m_IsInit)
        {
            inputPanel.SetIsManualFever(!DataHelper.isAutoFever);
        }

        // update the UI as in the method we're skipping
        __instance.RefreshRandomLevelConfigUI(false);

        // skip vanilla's code to actually switch the random mode completely
        return false;
    }
}
