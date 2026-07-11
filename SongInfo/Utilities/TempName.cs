using Il2Cpp;
using MelonLoader;
using UnityEngine;

using Bnfour.MuseDashMods.SongInfo.Data;
using Bnfour.MuseDashMods.SongInfo.Utilities.UI.Setting;

namespace Bnfour.MuseDashMods.SongInfo.Utilities;

// TODO think of a name
internal static class TempName
{
    // used in OnEnablePatch, panel ref available
    internal static void SetSongInfo(PnlPreparation panel, string bpm, string duration, bool animate = true)
    {
        IDataSetter dataSetter = Melon<SongInfoMod>.Instance.Layout switch
        {
            SongInfoLayout.OneLine => new TopRightSetterOneLine(),
            SongInfoLayout.TwoLines => new TopRightSetterTwoLines(),
            SongInfoLayout.BestRecord => new BestRecordPanelSetter(),
            _ => throw new System.ApplicationException("Unknown layout type")
        };

        dataSetter.Set(panel, bpm, duration, animate);
    }

    // used in load callback, panel unavailable
    // caller should check if the selected song didn't change (unlikely, but)
    internal static void SetSongInfoIfNeeded_Callback(string bpm, string duration)
    {
        var panel = GameObject.Find("UI/Standerd/PnlPreparation")?.GetComponent<PnlPreparation>();
        if (panel != null && panel.isActiveAndEnabled)
        {
            SetSongInfo(panel, bpm, duration, false);
        }
        else
        {
            Melon<SongInfoMod>.Logger.Msg("doing nothing");
        }
    }
}
