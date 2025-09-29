using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.Init))]
public class PnlRankInitPatch
{
    internal static void Postfix(PnlRank __instance)
    {
        var mod = Melon<ScoreboardCharactersMod>.Instance;
        if (!mod.OncePerSceneUpdatesApplied)
        {
            UiPatcher.FixRankPanelHeight(__instance);
            UiPatcher.CreateModUiForScoreboardEntry(__instance.server);

            mod.OncePerSceneUpdatesApplied = true;
        }
    }
}
