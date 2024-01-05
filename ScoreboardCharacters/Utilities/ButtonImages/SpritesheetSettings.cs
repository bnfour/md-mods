using SkiaSharp;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities.ButtonImages;

/// <summary>
/// Class that holds settings of the currently loaded spritesheet,
/// be it default builtin image or a custom override.
/// </summary>
public class SpritesheetSettings
{
    /// <summary>
    /// The image that contains the spritesheet.
    /// </summary>
    public readonly SKBitmap Bitmap;
    /// <summary>
    /// Size of an individual sprite in the spritesheet, in pixels.
    /// </summary>
    public readonly int SpriteSize;
    /// <summary>
    /// The rectangle to put character sprite in the button.
    /// The left square of the entire button sprite.
    /// </summary>
    public readonly SKRectI CharacterDest;
    /// <summary>
    /// The rectangle to put elfin sprite in the button.
    /// The right square of the entire button sprite.
    /// </summary>
    public readonly SKRectI ElfinDest;

    public SpritesheetSettings(SKBitmap bitmap, int spriteSize)
    {
        Bitmap = bitmap;
        SpriteSize = spriteSize;

        CharacterDest = new SKRectI(0, 0, SpriteSize, SpriteSize);
        ElfinDest = new SKRectI(SpriteSize, 0, 2 * SpriteSize, SpriteSize);
    }
}
