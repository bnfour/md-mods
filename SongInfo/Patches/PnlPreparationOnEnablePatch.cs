using HarmonyLib;
using MelonLoader;
using UnityEngine;

using Il2Cpp;
using Il2CppAssets.Scripts.Database;

using Bnfour.MuseDashMods.SongInfo.Data;
using UnityEngine.UI;

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
        var layout = Melon<SongInfoMod>.Instance.Layout;

        switch (layout)
        {
            // intentional fallthrough: same component, different components inside
            case SongInfoLayout.OneLine:
            case SongInfoLayout.TwoLines:
                var customObject = __instance.transform.Find(Constants.TopRight.Component);
                if (layout == SongInfoLayout.OneLine)
                {
                    customObject?.transform.Find(Constants.TopRight.OneLine)
                        ?.GetComponent<LongSongNameController>()
                        ?.Refresh($"{duration}, {bpm} BPM", delay: 0);
                }
                else
                {
                    customObject?.transform.Find(Constants.TopRight.TwoLinesBpm)
                        ?.GetComponent<LongSongNameController>()
                        ?.Refresh($"BPM: {bpm}", delay: 0);
                    customObject?.transform.Find(Constants.TopRight.TwoLinesDuration)
                        ?.GetComponent<LongSongNameController>()
                        ?.Refresh($"Length: {duration}", delay: 0);
                }
                var animation = customObject.GetComponent<Animation>();
                animation?.Play(animation.clip?.name);
                break;

            case SongInfoLayout.BestRecord:
                var bpmText = __instance.pnlRecord.transform
                    ?.Find(Constants.BestRecordPanel.BpmFullPath)
                    ?.GetComponent<Text>();
                if (bpmText != null)
                {
                    bpmText.text = bpm;
                }
                var durationText = __instance.pnlRecord.transform
                    ?.Find(Constants.BestRecordPanel.DurationFullPath)
                    ?.GetComponent<Text>();
                if (durationText != null)
                {
                    durationText.text = duration;
                }

                // TODO play animations
                break;
        }

        // for Custom Albums mod compatibility:
        // hide achievements in custom charts (uid start with 999), show in vanilla charts

        var isVanillaChart = info.uid[..3] != "999";
        __instance.stageAchievementValue.gameObject.SetActive(isVanillaChart);
        __instance.pnlPreparationLayAchv.transform.Find("ImgStageAchievement")?.gameObject.SetActive(isVanillaChart);
    }
}
