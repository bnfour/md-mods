using UnityEngine.UI;

using Il2Cpp;

using Bnfour.MuseDashMods.SongInfo.UI;

namespace Bnfour.MuseDashMods.SongInfo.Utilities.UI.Setting;

internal class BestRecordPanelSetter : IDataSetter
{
    public void Set(PnlPreparation panel, string bpm, string duration)
    {
        var bpmText = panel.pnlRecord.transform
            ?.Find(Constants.BestRecordPanel.BpmFullPath)
            ?.GetComponent<Text>();
        if (bpmText != null)
        {
            bpmText.text = bpm;
        }
        var durationText = panel.pnlRecord.transform
            ?.Find(Constants.BestRecordPanel.DurationFullPath)
            ?.GetComponent<Text>();
        if (durationText != null)
        {
            durationText.text = duration;
        }

        // TODO play animations
    }
}
