using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.UITweaks.Data;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

[HarmonyPatch(typeof(PnlBattle), nameof(PnlBattle.Awake))]
public class PnlBattleAwakePatch
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

        // TODO actually check
        var isTouhouMode = false;
        // do nothing to fever bar if touhou mode is enabled
        if (isTouhouMode && syncMode == HpFeverFlowSyncMode.FeverToHp)
        {
            return;
        }

        // TODO call some util class to change __instance.currentComps based on mode
    }
}
