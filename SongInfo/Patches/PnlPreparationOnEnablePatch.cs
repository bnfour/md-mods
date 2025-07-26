using HarmonyLib;
using Il2Cpp;
using MelonLoader;

using Il2CppAssets.Scripts.Database;

namespace Bnfour.MuseDashMods.SongInfo.Patches;

/// <summary>
/// Patch to fill in the custom data on opening the song screen.
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.OnEnable))]
public class PnlPreparationOnEnablePatch
{
    private static void Postfix(PnlPreparation __instance)
    {
        var info = GlobalDataBase.s_DbMusicTag.CurMusicInfo();
        var bpm = info.bpm;
        var duration = Melon<SongInfoMod>.Instance.DurationProvider.GetDuration(info);

        var customObject = __instance.transform.Find(Constants.CombinedStringComponentName);
        // update the text field with the data
        customObject?.transform.Find("ImgStageDesignerMask")
            ?.GetComponent<LongSongNameController>()
            ?.Refresh($"{duration}, {bpm} BPM", delay: 0);
        // TODO play the animation

        // for Custom Albums mod compatibility:
        // hide achievements in custom charts (uid start with 999), show in vanilla charts

        var isVanillaChart = info.uid[..3] != "999";
        __instance.stageAchievementValue.gameObject.SetActive(isVanillaChart);
        __instance.pnlPreparationLayAchv.transform.Find("ImgStageAchievement")?.gameObject.SetActive(isVanillaChart);
    }
}
