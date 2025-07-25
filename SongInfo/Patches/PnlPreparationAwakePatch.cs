using HarmonyLib;
using Il2Cpp;
using MelonLoader;
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

        var logger = Melon<SongInfoMod>.Logger;

        var bpmField = UnityEngine.Object.Instantiate(__instance.designerLongNameController,
            __instance.designerLongNameController.transform.parent);
        bpmField.name = Constants.BpmStringComponentName;
        bpmField.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1540, 0, 0);

        var durationField = UnityEngine.Object.Instantiate(__instance.designerLongNameController,
            __instance.designerLongNameController.transform.parent);
        durationField.name = Constants.DurationStringComponentName;
        durationField.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1540, -45, 0);
    }
}
