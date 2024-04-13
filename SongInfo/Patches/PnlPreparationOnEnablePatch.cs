using System.Diagnostics;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using MelonLoader;

namespace Bnfour.MuseDashMods.SongInfo.Patches;

/// <summary>
/// Patch to fill in the custom data on opening the song screen.
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.OnEnable))]
public class PnlPreparationOnEnablePatch
{
    private static void Postfix(PnlPreparation __instance)
    {
        // TODO don't forget to remove the watch
        var watch = new Stopwatch();
        watch.Start();

        var info = GlobalDataBase.s_DbMusicTag.CurMusicInfo();

        var bpm = info.bpm;
        var duration = Melon<SongInfoMod>.Instance.DurationProvider.GetDuration(info);
        watch.Stop();
        // TODO put in ui

        Melon<SongInfoMod>.Logger.Msg($"BPM {bpm}, {duration}. Done in {watch.ElapsedMilliseconds} ms");
    }
}
