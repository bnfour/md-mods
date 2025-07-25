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
            __instance.designerLongNameController.transform.parent);
        dataField.name = Constants.CombinedStringComponentName;
        dataField.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1540, 0, 0);
    }
}
