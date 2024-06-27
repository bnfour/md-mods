using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.SongInfo.Patches;

/// <summary>
/// Patch to fill in the custom data on opening the song screen.
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.OnEnable))]
public class PnlPreparationOnEnablePatch
{
    private static void Postfix()
    {
        var info = GlobalDataBase.s_DbMusicTag.CurMusicInfo();
        var bpm = info.bpm;
        var duration = Melon<SongInfoMod>.Instance.DurationProvider.GetDuration(info);

        var bpmField = GameObject.Find(Constants.BpmStringComponentName)
            ?.GetComponent<LongSongNameController>();
        bpmField?.Refresh($"BPM: {bpm}", delay: 0);

        var durationField = GameObject.Find(Constants.DurationStringComponentName)
            ?.GetComponent<LongSongNameController>();
        durationField?.Refresh($"Length: {duration}", delay: 0);
    }
}
