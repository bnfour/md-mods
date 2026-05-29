using SkiaSharp;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities.ButtonImages;

/// <summary>
/// Holds settings of the currently loaded spritesheet,
/// be it default builtin image or a custom override.
/// </summary>
/// <param name="Bitmap">The image that contains the main spritesheet.</param>
/// <param name="SpriteSize">Size of an individual sprite in the spritesheet, in pixels.</param>
public record SpritesheetSettings(SKBitmap Bitmap, int SpriteSize)
{
    /// <summary>
    /// The rectangle to put character sprite in the button.
    /// The left square of the entire button sprite.
    /// </summary>
    public readonly SKRectI CharacterDest = new(0, 0, SpriteSize, SpriteSize);
    /// <summary>
    /// The rectangle to put elfin sprite in the button.
    /// The right square of the entire button sprite.
    /// </summary>
    public readonly SKRectI ElfinDest = new(SpriteSize, 0, 2 * SpriteSize, SpriteSize);
}
