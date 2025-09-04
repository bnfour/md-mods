using UnityEngine.UI;

using Il2Cpp;

using Bnfour.MuseDashMods.SongInfo.UI;
using UnityEngine;

namespace Bnfour.MuseDashMods.SongInfo.Utilities.UI.Setting;

internal class BestRecordPanelSetter : IDataSetter
{
    public void Set(PnlPreparation panel, string bpm, string duration)
    {
        var animatableBpm = panel.pnlRecord.transform
            ?.Find(Constants.BestRecordPanel.BpmAnimationPath);

        var bpmText = animatableBpm
            ?.Find(Constants.BestRecordPanel.ValuePath)
            ?.GetComponent<Text>();
        if (bpmText != null)
        {
            bpmText.text = bpm;
        }

        var animatableDuration = panel.pnlRecord.transform
            ?.Find(Constants.BestRecordPanel.DurationAnimationPath);

        var durationText = animatableDuration
            ?.Find(Constants.BestRecordPanel.ValuePath)
            ?.GetComponent<Text>();
        if (durationText != null)
        {
            durationText.text = duration;
        }

        var bpmAnimation = animatableBpm?.GetComponent<Animation>();
        bpmAnimation?.Play(bpmAnimation?.clip?.name);
        var durationAnimation = animatableBpm?.GetComponent<Animation>();
        durationAnimation?.Play(durationAnimation?.clip?.name);
    }
}
