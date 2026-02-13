namespace Bnfour.MuseDashMods.FeverSwitch.Data;

/// <summary>
/// Types of custom sprites loaded from mod resources
/// to replace original ones.
/// </summary>
internal enum SpriteKind
{
    /// <summary>
    /// "Default" fever type, 42×42 monochrome icon.
    /// </summary>
    ToggleOff,
    /// <summary>
    /// "Modified" fever type, 128×128 gold medal image.
    /// </summary>
    ToggleOn,
    /// <summary>
    /// Custom hint for F based on PcButtonU, 64×64 white icon to be colored in code.
    /// </summary>
    Hint
}
