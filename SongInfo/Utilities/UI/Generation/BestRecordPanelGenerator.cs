using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;

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
        }

        // move and shrink the background for the free space of the panel
        var dankness = panel?.transform?.Find("Record/ImgBaseDarkP");
        if (dankness != null)
        {
            dankness.transform.position += VerticalOffset;
            var rt = dankness.GetComponent<RectTransform>();
            rt.sizeDelta += new Vector2(0, -100);
        }

        // TODO set up animations
    }
}
