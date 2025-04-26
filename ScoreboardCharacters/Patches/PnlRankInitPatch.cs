using HarmonyLib;

using Il2CppAssets.Scripts.UI.Panels;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

/// <summary>
/// A patch that switches to the scoreboard display when the panel is first shown.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.InitLevelConfigBtn))]
// i really wanted it to be an Init patch
// but whenever i try to patch that, the patch does not fire :(
public class PnlRankInitPatch
{
    internal static void Postfix(PnlRank __instance)
    {
        __instance.DoLevelConfigForward();
    }
}
