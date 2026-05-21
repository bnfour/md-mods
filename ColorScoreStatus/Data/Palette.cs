using UnityEngine;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Data;

internal record Palette
{
    // main color of the grade sprite
    internal Color Main { get; init; }
    // darkest color from the grade sprite -> HSV value reduced by 33 flat
    internal Color Outline { get; init; }
    // roughly defined as 75% between the main color and the lightest color from the reference S sprite
    internal Color GCGlow { get; init; }

    // based on gold S
    internal static Palette AllPerfect => new()
    {
        Main = new(1f, 0.941f, 0f),
        Outline = new(0.67f, 0.536f, 0f),
        GCGlow = new(1f, 0.984f, 0.643f)
    };
    // based on silver S
    internal static Palette FullCombo => new()
    {
        Main = new(0.8f, 0.941f, 0.996f),
        Outline = new(0.391f, 0.429f, 0.595f),
        GCGlow = new(0.945f, 0.984f, 1f)
    };
    // based on red S
    internal static Palette YouTried => new()
    {
        Main = new(1f, 0f, 0.529f),
        Outline = new(0.435f, 0.062f, 0.308f),
        GCGlow = new(1f, 0.514f, 0.863f)
    };
}
