using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;
using Il2CppInterop.Runtime;

using Bnfour.MuseDashMods.SongInfo.Patches;

namespace Bnfour.MuseDashMods.SongInfo.Utilities.UI.Generation;

/// <summary>
/// Base class for top right components: one line and two lines.
/// Contains the UI generation code shared between those two.
/// </summary>
internal abstract class TopRightGeneratorBase : IComponentGenerator
{
    /// <summary>
    /// Vertical offset of one text line. 1:1 with screen pixels on 1080 resolution.
    /// </summary>
    protected const float LineOffset = 45;

    /// <summary>
    /// The name of the scrolling text controller we make use of.
    /// </summary>
    protected const string ComponentToKeepAndClone = "ImgStageDesignerMask";

    /// <summary>
    /// The name to set for the clone of existing scrolling text component --
    /// the only one for one line layout, the first line for the two line layout.
    /// </summary>
    protected abstract string FirstLineComponentName { get; }

    public void CreateModUI(PnlPreparation instance)
    {
        var component = SetupEntireComponent(instance);

        if (component != null)
        {
            var originalScroller = component.transform?.Find(ComponentToKeepAndClone);
            if (originalScroller != null)
            {
                SetupTextFields(originalScroller);
            }

            SetupAnimation(component);
        }
    }

    protected GameObject SetupEntireComponent(PnlPreparation instance)
    {
        // clone the original component
        var dataField = GameObject.Instantiate(instance?.transform.Find("TxtStageDesigner")?.gameObject, instance.transform);
        if (dataField == null)
        {
            return null;
        }
        dataField.name = Constants.TopRight.Component;

        // move the clone to the right side of the screen and up a bit
        // so the second (scrollable) line of the clone is aligned with the first line of original
        var positionReference = instance.transform.Find("TxtStageDesigner")?.GetComponent<RectTransform>()?.anchoredPosition3D;
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

        return dataField;
    }

    protected virtual void SetupTextFields(Transform originalScrollerTransform)
    {
        originalScrollerTransform.name = FirstLineComponentName;
        originalScrollerTransform.gameObject.name = FirstLineComponentName;
    }

    protected void SetupAnimation(GameObject component)
    {
        var animation = component.AddComponent<Animation>();
        var clip = new AnimationClip()
        {
            legacy = true,
            name = "BnSongInfoTopRightClip"
        };
        // the curves are taken from the resources, the alpha matches perfectly, position -- kinda
        // alpha
        clip.SetCurve("", Il2CppType.Of<CanvasGroup>(), "m_Alpha", new(new(0, 0), new(1f / 15, 0), new(7f / 30, 1)));
        // position
        var originalPosition = component.GetComponent<RectTransform>().anchoredPosition.x;
        clip.SetCurve("", Il2CppType.Of<RectTransform>(), "m_AnchoredPosition.x", new(new(0, originalPosition - 100), new(1f / 15, originalPosition - 100), new(7f / 30, originalPosition)));

        animation.AddClip(clip, clip.name);
        animation.clip = clip;
    }
}
