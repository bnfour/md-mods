using UnityEngine;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Data;

internal record Palette
{
    internal Color Main { get; init; }
    internal Color Outline { get; init; }
    // roughly defined as 75% between the main color and the lightest color from the reference S sprite,
    // precalculated
    internal Color GCGlow { get; init; }

    // TODO consider darkening the outlines

    // based on gold S
    internal static Palette AllPerfect => new()
    {
        Main = new(1f, 0.941f, 0f),
        Outline = new(1f, 0.8f, 0f),
        GCGlow = new(1f, 0.984f, 0.643f)
    };
    // based on silver S
    internal static Palette FullCombo => new()
    {
        Main = new(0.8f, 0.941f, 0.996f),
        Outline = new(0.608f, 0.667f, 0.925f),
        GCGlow = new(0.945f, 0.984f, 1f)
    };
    // based on red S
    internal static Palette YouTried => new()
    {
        Main = new(1f, 0f, 0.529f),
        Outline = new(0.765f, 0.11f, 0.541f),
        GCGlow = new(1f, 0.514f, 0.863f)
    };
}
