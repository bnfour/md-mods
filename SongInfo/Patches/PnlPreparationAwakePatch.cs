using HarmonyLib;
using Il2Cpp;
using UnityEngine;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.SongInfo.Patches;

/// <summary>
/// Patch to modify the UI of preparation panel to include a textfield for BPM and duration.
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.Awake))]
public class PnlPreparationAwakePatch
{
    private static void Postfix(PnlPreparation __instance)
    {
        var dataField = GameObject.Instantiate(__instance?.transform.Find("TxtStageDesigner")?.gameObject,
            __instance.transform);
        dataField.name = Constants.CombinedStringComponentName;
        // move to the right side of the screen and up a bit
        // so the second scrollable line of the clone is aligned with the first line of original
        var positionReference = __instance.transform.Find("TxtStageDesigner")?.GetComponent<RectTransform>().anchoredPosition3D;
        if (positionReference != null)
        {
            dataField.GetComponent<RectTransform>().anchoredPosition3D = positionReference.Value + new Vector3(1540, 45, 0);
        }
        // hide the static text -- we don't need it here
        dataField.GetComponent<Text>().color = Color.clear;

        // TODO set up the animation like the other mod
    }
}
