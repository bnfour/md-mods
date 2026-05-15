using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.GameCore.HostComponent;
using Il2CppPeroPeroGames.GlobalDefines;

using Bnfour.MuseDashMods.ColorScoreStatus.Data;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Patches;

[HarmonyPatch(typeof(TaskStageTarget), nameof(TaskStageTarget.InvokeJudgeEvent))]
public class TaskStageTargetInvokeJudgeEventPatch
{
    //                                             [sic]
    private const uint Perfect = (uint)TaskResult.Prefect;
    private const uint Miss = (uint)TaskResult.Miss;

    internal static void Postfix(uint result, TaskStageTarget __instance)
    {
        var mod = Melon<ColorScoreStatusMod>.Instance;

        // first check for AP drop
        if (mod.Status == ComboStatus.AllPerfect
            && result < Perfect)
        {
            mod.Status = ComboStatus.FullCombo;
        }
        // missing things that do not drop combo (hearts/notes/etc)
        // is still reported as a miss judgement, so we also check the in-game 
        // counter for actual combo drops (the game uses the same very field to 
        // check for the full combo)
        if (mod.Status != ComboStatus.ThereWasAnAttempt
            && result == Miss && __instance.m_MissCombo > 0)
        {
            mod.Status = ComboStatus.ThereWasAnAttempt;
        }
    }
}
