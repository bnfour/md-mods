using UnityEngine;

using Bnfour.MuseDashMods.ColorScoreStatus.Data;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Utilities.ColorSetting;

internal abstract class ScoreColorerBase(GameObject reference) : IScoreColorer
{
    /// <summary>
    /// Object that holds score components to modify.
    /// </summary>
    protected readonly GameObject _reference = reference;

    protected Palette ForStatus(ComboStatus status)
        => status switch
    {
        ComboStatus.AllPerfect => Palette.AllPerfect,
        ComboStatus.FullCombo => Palette.FullCombo,
        ComboStatus.ThereWasAnAttempt => Palette.YouTried,
        _ => throw new System.ApplicationException("Unknown combo status.")
    };

    public abstract void SetStateTo(ComboStatus status);
}
