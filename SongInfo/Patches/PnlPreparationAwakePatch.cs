using HarmonyLib;
using Il2Cpp;
using UnityEngine;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.SongInfo.Patches;

/// <summary>
/// Patch to modify the UI of preparation panel to include textfields
/// for BPM and duration.
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.Awake))]
public class PnlPreparationAwakePatch
{
    private static void Postfix(PnlPreparation __instance)
    {
        // clone the song designer string twice to display bpm and duration,
        // place the clones on the right side of the screen

        var bpmField = UnityEngine.Object.Instantiate(__instance.designerLongNameController,
            __instance.designerLongNameController.transform.parent);
        bpmField.name = Constants.BpmStringComponentName;
        bpmField.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1540, 0, 0);

        var durationField = UnityEngine.Object.Instantiate(__instance.designerLongNameController,
            __instance.designerLongNameController.transform.parent);
        durationField.name = Constants.DurationStringComponentName;
        durationField.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1540, -45, 0);

        // move and restyle the achievements display as its original place
        // is taken by the new UI
        // TODO fine-tune the positions

        var achievementsText = __instance.stageAchievementValue;
        achievementsText.transform.SetParent(__instance.pnlPreparationLayAchv.transform);
        achievementsText.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-40, 357, 0);

        var achievementsPanelHeader = GameObject.Find("TxtContent").GetComponent<Text>();
        achievementsText.fontSize = achievementsPanelHeader.fontSize;
        achievementsText.color = achievementsPanelHeader.color;
        achievementsText.fontStyle = achievementsPanelHeader.fontStyle;

        var awardIcon = GameObject.Find("ImgStageAchievement");
        awardIcon.transform.SetParent(__instance.pnlPreparationLayAchv.transform);
        awardIcon.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-60, 357, 0);
    }
}
