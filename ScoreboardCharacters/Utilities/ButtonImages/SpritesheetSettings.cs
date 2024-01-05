using System.Drawing;

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
    public readonly Bitmap Bitmap;
    /// <summary>
    /// Size of an individual sprite in the spritesheet, in pixels.
    /// </summary>
    public readonly int SpriteSize;
    /// <summary>
    /// The rectangle to put character sprite in the button.
    /// The left square of the entire button sprite.
    /// </summary>
    public readonly Rectangle CharacterDest;
    /// <summary>
    /// The rectangle to put elfin sprite in the button.
    /// The right square of the entire button sprite.
    /// </summary>
    public readonly Rectangle ElfinDest;

    public SpritesheetSettings(Bitmap bitmap, int spriteSize)
    {
        Bitmap = bitmap;
        SpriteSize = spriteSize;

        CharacterDest = new Rectangle(0, 0, SpriteSize, SpriteSize);
        ElfinDest = new Rectangle(SpriteSize, 0, SpriteSize, SpriteSize);
    }
}
