using UnityEngine;
using UnityEngine.UI;

using Bnfour.MuseDashMods.ColorScoreStatus.Data;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Utilities.ColorSetting;

/// <summary>
/// Utility class to change colors of scores represented as <see cref="Text"/>
/// components, with an optional outline: default UI (has outline), DJMAX and Arknights UIs.
/// </summary>
/// <param name="defaultOrDjmaxOrArknights">GameObject that has the score text component.</param>
internal class BasicScoreColorer(GameObject defaultOrDjmaxOrArknights)
    : ScoreColorerBase(defaultOrDjmaxOrArknights), IScoreColorer
{
    public override void SetStateTo(ComboStatus status)
    {
        // TODO palette resolution
        _reference.GetComponent<Text>()?.color = Color.cyan;
        // only the default UI has text outline
        _reference.GetComponent<Outline>()?.effectColor = Color.magenta;
    }
}
