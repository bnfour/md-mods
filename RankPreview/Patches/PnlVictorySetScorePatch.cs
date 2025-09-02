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
        var mod = Melon<RankPreviewMod>.Instance;

        var ourScore = Singleton<TaskStageTarget>.instance.GetScore();

        var trackUid = GlobalDataBase.s_DbBattleStage.selectedMusicInfo.uid;
        var mapDifficulty = GlobalDataBase.s_DbBattleStage.m_MapDifficulty;
        var key = $"{trackUid}_{mapDifficulty}";

        // TODO update the patched ui instead of logging when available
        mod.LoggerInstance.Msg(mod.Cache.EstimateRank(key, ourScore));
    }
}
