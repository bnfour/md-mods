using System;
using HarmonyLib;
using MelonLoader;

using Il2Cpp;
using Il2CppAssets.Scripts.Database;

using Bnfour.MuseDashMods.SongInfo.Data;
using Bnfour.MuseDashMods.SongInfo.Utilities.UI.Setting;

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

        IDataSetter dataSetter = Melon<SongInfoMod>.Instance.Layout switch
        {
            SongInfoLayout.OneLine => new TopRightSetterOneLine(),
            SongInfoLayout.TwoLines => new TopRightSetterTwoLines(),
            SongInfoLayout.BestRecord => new BestRecordPanelSetter(),
            _ => throw new ApplicationException("Unknown layout type")
        };

        dataSetter.Set(__instance, bpm, duration);

        // for Custom Albums mod compatibility:
        // hide achievements in custom charts (uid start with 999), show in vanilla charts

        var isVanillaChart = info.uid[..3] != "999";
        __instance.stageAchievementValue.gameObject.SetActive(isVanillaChart);
        __instance.pnlPreparationLayAchv.transform.Find("ImgStageAchievement")?.gameObject.SetActive(isVanillaChart);
    }
}
