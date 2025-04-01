using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.UI.Panels;
using Il2CppAssets.Scripts.Database;
using Il2CppPeroPeroGames.GlobalDefines;

using Bnfour.MuseDashMods.UITweaks.Utilities;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Patch that replaces the "FEVER" sprite on the fever bar with "AUTO"
/// if it's enabled in the config and actually needed. 
/// </summary>
[HarmonyPatch(typeof(PnlBattle), nameof(PnlBattle.GameStart))]
public class PnlBattleGameStartPatch_AutoFeverNotice
{
    private static void Postfix(PnlBattle __instance)
    {
        // first check if the feature is enabled at all,
        // some levels have different ui,
        // touhou mode uses different sprite and has no fever,
        // finally check if the sprite needs to be changed
        if (!Melon<UITweaksMod>.Instance.AutoFeverNoticeEnabled
            || MusicUidChecker.IsMemeLevel()
            || GlobalDataBase.s_DbTouhou.isTouhouEasterEgg
            || !DataHelper.isAutoFever)
        {
            return;
        }

        FeverTextTextureReplacer.Replace(__instance);
    }
}
