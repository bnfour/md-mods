using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.UITweaks.Data;
using Bnfour.MuseDashMods.UITweaks.Utilities;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Patch that calls the bar changes on game start, if needed.
/// </summary>
[HarmonyPatch(typeof(PnlBattle), nameof(PnlBattle.GameStart))]
public class PnlBattleGameStartPatch
{
    private static void Postfix(PnlBattle __instance)
    {
        var modInstance = Melon<UITweaksMod>.Instance;
        if (!modInstance.HpFeverFlowSyncEnabled)
        {
            return;
        }

        var syncMode = modInstance.HpFeverFlowSyncUseAltMode
            ? HpFeverFlowSyncMode.HpToFever
            : HpFeverFlowSyncMode.FeverToHp;

        // do nothing to fever bar if touhou mode is enabled
        if (GlobalDataBase.s_DbTouhou.isTouhouEasterEgg
            && syncMode == HpFeverFlowSyncMode.FeverToHp)
        {
            return;
        }

        HpFeverBarsSynchronizer.Sync(__instance.currentComps, syncMode);
    }
}
