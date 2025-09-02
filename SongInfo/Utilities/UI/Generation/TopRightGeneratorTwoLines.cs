using UnityEngine;

using Bnfour.MuseDashMods.SongInfo.Patches;

namespace Bnfour.MuseDashMods.SongInfo.Utilities.UI.Generation;

/// <summary>
/// Generator for the classic two lines layout. Most of the work is handled by the base class,
/// this class only adds the second line specific for the layout.
/// </summary>
internal sealed class TopRightGeneratorTwoLines : TopRightGeneratorBase, IComponentGenerator
{
    protected override string FirstLineComponentName => Constants.TopRight.TwoLinesBpm;

    /// <summary>
    /// In addition to base setup, adds a second line for the duration below the original one.
    /// </summary>
    protected override void SetupTextFields(Transform originalScrollerTransform)
    {
        base.SetupTextFields(originalScrollerTransform);

        var secondLine = GameObject.Instantiate(originalScrollerTransform.gameObject, originalScrollerTransform.transform.parent);

        secondLine.name = Constants.TopRight.TwoLinesDuration;
        secondLine.transform.name = Constants.TopRight.TwoLinesDuration;

        var transform = secondLine.GetComponent<RectTransform>();
        if (transform != null)
        {
            transform.anchoredPosition3D += new Vector3(0, -LineOffset, 0);
        }
    }
}
