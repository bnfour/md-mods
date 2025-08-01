using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using MelonLoader;
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

        var animation = dataField.AddComponent<Animation>();
        var clip = new AnimationClip()
        {
            legacy = true,
            name = "BnSongInfoOneLinerClip"
        };
        // the curves are taken from the resources, the alpha matches perfectly, position -- kinda
        // alpha
        clip.SetCurve("", Il2CppType.Of<CanvasGroup>(), "m_Alpha", new(new(0, 0), new(1f / 15, 0), new (7f / 30, 1)));
        // position
        var originalPosition = dataField.GetComponent<RectTransform>().anchoredPosition.x;
        clip.SetCurve("", Il2CppType.Of<RectTransform>(), "m_AnchoredPosition.x", new(new(0, originalPosition - 100), new(1f / 15, originalPosition - 100), new(7f / 30, originalPosition)));

        animation.AddClip(clip, clip.name);
        animation.clip = clip;
    }
}
