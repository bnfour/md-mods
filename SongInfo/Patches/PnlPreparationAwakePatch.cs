using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
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
    private static GameObject AchievementsTextObj { get; set; }
    private static GameObject AwardIcon { get; set; }

    public static void SetAchievementsVisibility()
    {
        var isCustomAlbum = GlobalDataBase.s_DbMusicTag.CurMusicInfo().uid.StartsWith("999");
        AchievementsTextObj.SetActive(!isCustomAlbum);
        AwardIcon.SetActive(!isCustomAlbum);
    }
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

        var achievementsText = __instance.stageAchievementValue;
        achievementsText.transform.SetParent(__instance.pnlPreparationLayAchv.transform);
        achievementsText.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-45, 355, 0);

        var achievementsPanelHeader = GameObject.Find("TxtContent").GetComponent<Text>();
        achievementsText.fontSize = achievementsPanelHeader.fontSize;
        achievementsText.color = achievementsPanelHeader.color;
        achievementsText.fontStyle = achievementsPanelHeader.fontStyle;
        AchievementsTextObj = achievementsText.gameObject;

        AwardIcon = __instance.transform.Find("ImgStageAchievement").gameObject;
        AwardIcon.transform.SetParent(__instance.pnlPreparationLayAchv.transform);

        var rectTransform = AwardIcon.GetComponent<RectTransform>();
        rectTransform.anchoredPosition3D = new Vector3(-109, 355, 0);
        // for some reason X coordinate assignment refuses to work (see #11),
        // so an "alternative" way to move it horizontally is used
        // (no idea which anchor does the trick, maybe both?)
        rectTransform.anchorMax = new Vector2(1.05f, 0.5f);
        rectTransform.anchorMin = new Vector2(1.05f, 0.5f);
        // in 1080 resolution, 1px of moving the image is 0.00125 of anchor
        
        SetAchievementsVisibility();
    }
}
