using UnityEngine;

using Bnfour.MuseDashMods.ColorScoreStatus.Data;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Utilities.ColorSetting;

internal abstract class ScoreColorerBase(GameObject reference) : IScoreColorer
{
    /// <summary>
    /// Object that holds score components to modify.
    /// </summary>
    protected readonly GameObject _reference = reference;

    public abstract void SetStateTo(ComboStatus status);
}
