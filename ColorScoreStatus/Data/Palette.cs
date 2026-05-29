using UnityEngine;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Data;

/// <summary>
/// Holds colors used to change the score text color.
/// </summary>
/// <param name="Main">Main score color. Taken straight from the reference grade sprite.</param>
/// <param name="Outline">Outline color (when applicable). Darkest color from the reference sprite,
/// has its HSV value reduced by additional 33 (in 0–100 range).</param>
/// <param name="GCGlow">Glow color for Groove Coaster text effects.
/// 3/4 on the gradient from main color to reference sprite's lightest color.</param>
internal record Palette(Color Main, Color Outline, Color GCGlow)
{
    /// <summary>
    /// Based on golden S sprite. Used for AP scores.
    /// </summary>
    internal static Palette AllPerfect => new(
        new(1f, 0.941f, 0f),
        new(0.67f, 0.536f, 0f),
        new(1f, 0.984f, 0.643f)
    );
    /// <summary>
    /// Based on silver S sprite. Used for non-AP FC scores.
    /// </summary>
    internal static Palette FullCombo => new(
        new(0.8f, 0.941f, 0.996f),
        new(0.391f, 0.429f, 0.595f),
        new(0.945f, 0.984f, 1f)
    );
    /// <summary>
    /// Based on red S sprite. Used for non-FC scores.
    /// </summary>
    internal static Palette YouTried => new(
        new(1f, 0f, 0.529f),
        new(0.435f, 0.062f, 0.308f),
        new(1f, 0.514f, 0.863f)
    );
}
