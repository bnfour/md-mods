using Il2Cpp;
using MelonLoader;
using UnityEngine;

using Bnfour.MuseDashMods.SongInfo.Data;
using Bnfour.MuseDashMods.SongInfo.Utilities.UI.Setting;

namespace Bnfour.MuseDashMods.SongInfo.Utilities;

/// <summary>
/// Holds methods to actually display the data on UI.
/// </summary>
internal static class SetInfoDispatcher
{
    // used by itself in OnEnable patch, where panel ref is available
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

    // used in resource load callback, panel ref unavailable...
    internal static void SetSongInfoIfNeeded_Callback(string bpm, string duration)
    {
        // ...so we search for it themselves
        var panel = GameObject.Find("UI/Standerd/PnlPreparation")?.GetComponent<PnlPreparation>();
        if (panel != null && panel.isActiveAndEnabled)
        {
            SetSongInfo(panel, bpm, duration, false);
        }
    }
}
