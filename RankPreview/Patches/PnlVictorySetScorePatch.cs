using System.Linq;
using HarmonyLib;
using MelonLoader;

using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.GameCore.HostComponent;

namespace Bnfour.MuseDashMods.RankPreview.Patches;

[HarmonyPatch(typeof(PnlVictory), nameof(PnlVictory.SetScore))]
public class PnlVictorySetScorePatch
{
    internal static void Postfix(PnlVictory __instance)
    {
        // TODO update the patched ui instead of logging when available
        var mod = Melon<RankPreviewMod>.Instance;

        var ourScore = Singleton<TaskStageTarget>.instance.GetScore();

        var trackUid = GlobalDataBase.s_DbBattleStage.selectedMusicInfo.uid;
        var mapDifficulty = GlobalDataBase.s_DbBattleStage.m_MapDifficulty;
        var key = $"{trackUid}_{mapDifficulty}";

        if (mod.Cache.ContainsKey(key))
        {
            var estimatedRank = mod.Cache[key].TakeWhile(score => score >= ourScore).Count() + 1;
            if (estimatedRank <= 99)
            {
                mod.LoggerInstance.Msg($"a score of {ourScore} would yield rank {estimatedRank} based on cached data");
            }
            else
            {
                mod.LoggerInstance.Msg("git gud");
            }
        }
        else
        {
            mod.LoggerInstance.Msg("¯\\_(ツ)_/¯ (scoreboard not loaded?)");
        }
    }
}
