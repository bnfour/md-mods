using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace Bnfour.MuseDashMods.SongInfo.Patches;

/// <summary>
/// Patch to modify the UI of preparation panel to include a textfield for BPM and duration.
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.Awake))]
public class PnlPreparationAwakePatch
{
    private static void Postfix(PnlPreparation __instance)
    {
        // clone the song designer string to display bpm and duration,
        // place the clone on the right side of the screen
        var dataField = GameObject.Instantiate(__instance.designerLongNameController,
            __instance.transform);
        dataField.name = Constants.CombinedStringComponentName;

        var positionReference = __instance.transform.Find("TxtStageDesigner")?.GetComponent<RectTransform>().anchoredPosition3D;
        if (positionReference != null)
        {
            dataField.GetComponent<RectTransform>().anchoredPosition3D = positionReference.Value + new Vector3(1540, 0, 0);
        }
    }
}
