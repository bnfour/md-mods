using UnityEngine;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Data;

internal record Palette
{
    internal Color Main { get; init; }
    internal Color Outline { get; init; }

    // TODO GC props as needed

    // TODO consider darkening the outlines

    // based on gold S
    internal static Palette AllPerfect => new()
    {
        Main = new Color(1f, 0.941f, 0f),
        Outline = new Color(1f, 0.8f, 0f)
    };
    // based on silver S
    internal static Palette FullCombo => new()
    {
        Main = new Color(0.8f, 0.941f, 0.996f),
        Outline = new Color(0.608f, 0.667f, 0.925f)
    };
    // based on red S
    internal static Palette YouTried => new()
    {
        Main = new Color(1f, 0f, 0.529f),
        Outline = new Color(0.765f, 0.11f, 0.541f)
    };
}
