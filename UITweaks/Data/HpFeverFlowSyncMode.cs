namespace Bnfour.MuseDashMods.UITweaks.Data;

/// <summary>
/// Represents a way used to sync the two bars.
/// </summary>
internal enum HpFeverFlowSyncMode
{
    /// <summary>
    /// Default mode: modify the fever bar to match default HP bar.
    /// </summary>
    FeverToHp = 0,
    /// <summary>
    /// Alternate mode: modify the HP bar to match default fever bar.
    /// </summary>
    HpToFever
}
