using HarmonyLib;

using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;
/// <summary>
/// A patch that alters the scoreboard UI
/// and switches to the scoreboard display when the panel is first shown.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.InitLevelConfigBtn))]
// i really wanted it to be an Init patch
// but whenever i try to patch that, the patch does not fire :(
public class PnlRankInitLevelConfigBtnPatch
{
    internal static void Postfix(PnlRank __instance)
    {
        UiPatcher.MinifyTopLevelConfigUi(__instance);
        // open the extended scoreboard as soon as the screen is seen
        __instance.DoLevelConfigForward();
    }
}
