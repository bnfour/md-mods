using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.GameCore.HostComponent;
using Il2CppAssets.Scripts.PeroTools.Commons;

using Bnfour.MuseDashMods.RankPreview.Data;

namespace Bnfour.MuseDashMods.RankPreview.Patches;

[HarmonyPatch(typeof(PnlVictory), nameof(PnlVictory.SetScore))]
public class PnlVictorySetScorePatch
{
    internal static void Postfix(PnlVictory __instance)
    {
        var ourScore = Singleton<TaskStageTarget>.instance.GetScore();

        var trackUid = GlobalDataBase.s_DbBattleStage.selectedMusicInfo.uid;
        var mapDifficulty = GlobalDataBase.s_DbBattleStage.m_MapDifficulty;
        var key = $"{trackUid}_{mapDifficulty}";

        // TODO looks scuffed, a better way to find the component
        // currently we traverse the hierarchy like this:
        // an actual score text (a number)
        // -> its parent, score text (which we cloned)
        //   -> its parent, PnlVictory_3D
        //     -> its child, _the_ component, by its name
        var textfield = __instance?.m_CurControls?.scoreTxt?.transform.parent.parent.Find(Constants.ExtraComponentName);
        if (textfield != null)
        {
            var text = textfield.GetComponent<Text>();
            if (text != null)
            {
                text.text = Melon<RankPreviewMod>.Instance.Cache.EstimateRank(key, ourScore);
            }
            var animation = textfield.GetComponent<Animation>();
            animation?.Play(animation.clip?.name);
        }
    }
}
