namespace Bnfour.MuseDashMods.ColorScoreStatus.Data;

/// <summary>
/// In-game states that are distinct to the mod and are mapped to score colors.
/// </summary>
public enum ComboStatus
{
    /// <summary>
    /// All hits are perfect.
    /// </summary>
    AllPerfect = 0,
    /// <summary>
    /// All notes are hit, but not all are perfect, or missable things that don't
    /// affect the combo are missed.
    /// </summary>
    FullCombo,
    /// <summary>
    /// Also known as "git gud".
    /// </summary>
    ThereWasAnAttempt
}
