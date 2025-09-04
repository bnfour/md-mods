using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;
using Il2CppInterop.Runtime;

using Bnfour.MuseDashMods.SongInfo.UI;

namespace Bnfour.MuseDashMods.SongInfo.Utilities.UI.Generation;

internal class BestRecordPanelGenerator : IComponentGenerator
{
    // seems to use rows as coordinates
    private static readonly Vector3 VerticalOffset = new(0, -1, 0);

    public void CreateModUI(PnlPreparation instance)
    {
        var panel = instance.pnlRecord;

        // clone the accuracy field for a bpm field
        var accuracyFieldTransform = panel?.transform?.Find("Record/ImgBaseAccuracy");
        if (accuracyFieldTransform != null)
        {
            var bpmField = GameObject.Instantiate(accuracyFieldTransform.gameObject, accuracyFieldTransform.parent);
            bpmField.name = Constants.BestRecordPanel.BpmComponent;
            bpmField.transform.name = Constants.BestRecordPanel.BpmComponent;
            bpmField.transform.position += new Vector3(0, -1, 0);

            var bpmTitle = bpmField.transform.Find("TxtAccuracy");
            bpmTitle.name = Constants.BestRecordPanel.BpmTxt;
            bpmTitle.transform.name = Constants.BestRecordPanel.BpmTxt;
            bpmTitle.GetComponent<Text>().text = "BPM";

            SetupAnimation(bpmTitle, 7);
        }

        // clone the times clear field for a duration field
        var clearFieldTransform = panel.transform.Find("Record/ImgBaseClear");
        if (clearFieldTransform != null)
        {
            var durationField = GameObject.Instantiate(clearFieldTransform.gameObject, clearFieldTransform.parent);
            durationField.name = Constants.BestRecordPanel.DurationComponent;
            durationField.transform.name = Constants.BestRecordPanel.DurationComponent;
            durationField.transform.position += new Vector3(0, -1, 0);

            var durationTitle = durationField.transform.Find("TxtClear");
            durationTitle.name = Constants.BestRecordPanel.DurationTxt;
            durationTitle.transform.name = Constants.BestRecordPanel.DurationTxt;
            durationTitle.GetComponent<Text>().text = "DURATION";

            SetupAnimation(durationTitle, 8);
        }

        // move and shrink the background for the free space of the panel
        var dankness = panel?.transform?.Find("Record/ImgBaseDarkP");
        if (dankness != null)
        {
            dankness.transform.position += VerticalOffset;
            var rt = dankness.GetComponent<RectTransform>();
            rt.sizeDelta += new Vector2(0, -100);
        }
    }

    private void SetupAnimation(Transform canvasGroupHolder, int orderDelay)
    {
        // animations of existing texts from the panel start after each other
        // with a 1/30 s delay, and every animation is 1/6 s long
        // this is a mimicry for that, though i'm unsure about the order:
        // - there are 7 animations in a row with {0..6}/30 s delays defined
        // - there are 5 texts to animate

        var animation = canvasGroupHolder.gameObject.AddComponent<Animation>();
        AnimationClip clip = new()
        {
            legacy = true,
            name = $"BnSongInfoRecordPanelText{orderDelay}"
        };
        // both values to animate seem to follow the same timing
        var startDelay = (float)orderDelay / 30;
        var animationEndTime = startDelay + 1f / 6;
        // alpha 0 -> 1, anchoredPosition.y 30 -> 0
        clip.SetCurve("", Il2CppType.Of<CanvasGroup>(), "m_Alpha",
            new(new(0, 0), new(startDelay, 0), new(animationEndTime, 1)));
        clip.SetCurve("", Il2CppType.Of<RectTransform>(), "m_AnchoredPosition.y",
            new(new(0, 30), new(startDelay, 30), new(animationEndTime, 0)));

        animation.AddClip(clip, clip.name);
        animation.clip = clip;
    }
}
