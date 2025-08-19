using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;
using Il2CppInterop.Runtime;

using Bnfour.MuseDashMods.SongInfo.Data;

namespace Bnfour.MuseDashMods.SongInfo.Patches;

/// <summary>
/// Patch to modify the UI of preparation panel to include a textfield for BPM and duration.
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.Awake))]
public class PnlPreparationAwakePatch
{
    // TODO consider moving actual UI manipulation outside of a patch class
    private const float LineOffset = 45;
    private const string ComponentToKeepAndClone = "ImgStageDesignerMask";

    private static void Postfix(PnlPreparation __instance)
    {
        var layout = Melon<SongInfoMod>.Instance.Layout;

        var dataField = GameObject.Instantiate(__instance?.transform.Find("TxtStageDesigner")?.gameObject,
            __instance.transform);
        dataField.name = Constants.TopRightComponentName;
        // move to the right side of the screen and up a bit
        // so the second scrollable line of the clone is aligned with the first line of original
        var positionReference = __instance.transform.Find("TxtStageDesigner")?.GetComponent<RectTransform>().anchoredPosition3D;
        if (positionReference != null)
        {
            dataField.GetComponent<RectTransform>().anchoredPosition3D = positionReference.Value + new Vector3(1540, LineOffset, 0);
        }
        // remove the "level design:" text -- we don't need it
        var textToRemove = dataField.GetComponent<Text>();
        GameObject.DestroyImmediate(textToRemove);
        // remove any other child components other mods might have added to the original
        // that ended up in our clone (#24)
        var initialChildCount = dataField.transform.childCount;
        for (int i = initialChildCount - 1; i >= 0; i--)
        {
            var child = dataField.transform.GetChild(i);
            if (child.gameObject.name != ComponentToKeepAndClone)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
        // rename the cloned scrolled text depending on the mode used
        var originalScroller = dataField?.transform?.Find(ComponentToKeepAndClone);
        if (originalScroller != null)
        {
            var name = layout switch
            {
                SongInfoLayout.OneLine => Constants.OneLineComponentName,
                SongInfoLayout.TwoLines => Constants.TwoLinesBpmComponentName,
                _ => throw new System.ApplicationException("Unsupported layout type, _should_ never happen.")
            };
            originalScroller.name = name;
            originalScroller.gameObject.name = name;
        }
        // add another below if needed
        if (layout == SongInfoLayout.TwoLines && originalScroller != null)
        {
            var secondLine = GameObject.Instantiate(originalScroller.gameObject, originalScroller.transform.parent);

            secondLine.name = Constants.TwoLinesDurationComponentName;
            secondLine.transform.name = Constants.TwoLinesDurationComponentName;

            var transform = secondLine.GetComponent<RectTransform>();
            if (transform != null)
            {
                transform.anchoredPosition3D += new Vector3(0, -LineOffset, 0);
            }
        }

        // add animation to the entire component
        var animation = dataField.AddComponent<Animation>();
        var clip = new AnimationClip()
        {
            legacy = true,
            name = "BnSongInfoTopRightClip"
        };
        // the curves are taken from the resources, the alpha matches perfectly, position -- kinda
        // alpha
        clip.SetCurve("", Il2CppType.Of<CanvasGroup>(), "m_Alpha", new(new(0, 0), new(1f / 15, 0), new(7f / 30, 1)));
        // position
        var originalPosition = dataField.GetComponent<RectTransform>().anchoredPosition.x;
        clip.SetCurve("", Il2CppType.Of<RectTransform>(), "m_AnchoredPosition.x", new(new(0, originalPosition - 100), new(1f / 15, originalPosition - 100), new(7f / 30, originalPosition)));

        animation.AddClip(clip, clip.name);
        animation.clip = clip;
    }
}
