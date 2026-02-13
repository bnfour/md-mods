using SkiaSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities.ButtonImages;

/// <summary>
/// Encapsulates loading (and reloading) spritesheets and related settings.
/// </summary>
public class SpritesheetManager
{
    private const string OverrideFilename = "scoreboard_characters_override.png";
    private readonly int[] SupportedResolutions = [720, 1080, 1440, 2160];
    // 1920×1080 is the baseline resolution
    private const int BaseResolution = 1080;
    private const int BaseSpriteSize = 40;

    private int _currentResolution;
    private bool _overrideActive;

    private bool _initialized;

    public SpritesheetSettings LoadSpritesheet()
    {
        _initialized = true;
        _currentResolution = Screen.height;

        var spriteSize = (int)Math.Round(BaseSpriteSize * (decimal)_currentResolution / BaseResolution, MidpointRounding.ToEven);

        return new(LoadOverrideSpritesheet() ?? LoadDefaultSpritesheet(spriteSize), spriteSize);
    }

    public bool ReloadRequired()
    {
        return !_initialized && !_overrideActive && Screen.height != _currentResolution;
    }

    /// <summary>
    /// Tries to load the override image, if it contains square sprites with the same size, <see cref="Constants.SpritesPerRow"/> per row.
    /// </summary>
    /// <returns> A bitmap if the override was successfully loaded, null if the override was not loaded for any reason
    /// (it doesn't exist or the resolution is wrong). </returns>
    /// <remarks>It only checks image dimensions, not the contents.
    /// Also sets up <see cref="_overrideActive"/> flag on success.</remarks>
    private SKBitmap LoadOverrideSpritesheet()
    {
        var overrideFullPath = Path.Combine(Application.dataPath, OverrideFilename);
        if (File.Exists(overrideFullPath))
        {
            var overrideBitmap = SKImage.FromEncodedData(overrideFullPath);
            if (overrideBitmap.Width % Constants.SpritesPerRow != 0)
            {
                MelonLoader.Melon<ScoreboardCharactersMod>.Logger.Warning($"The width of the override image ({overrideBitmap.Width}) is not divisible by {Constants.SpritesPerRow}. The override will not be applied.");
                return null;
            }
            int potentialOverrideSpriteSize = overrideBitmap.Width / Constants.SpritesPerRow;
            if (overrideBitmap.Height % potentialOverrideSpriteSize != 0)
            {
                MelonLoader.Melon<ScoreboardCharactersMod>.Logger.Warning($"The height of the override image ({overrideBitmap.Height}) is not divisible by sprite size ({potentialOverrideSpriteSize}). The override will not be applied.");
                return null;
            }

            _overrideActive = true;
            return SKBitmap.FromImage(overrideBitmap);
        }

        return null;
    }

    /// <summary>
    /// Creates bitmap from built-in spritesheet.
    /// If no pre-scaled version is available in the resources, on the fly rescale to target sprite size is performed.
    /// </summary>
    private SKBitmap LoadDefaultSpritesheet(int targetSpriteSize)
        => LoadDefaultImage("sprites", targetSpriteSize, RescaleSpritesheet);

    /// <summary>
    /// Loads image file from embedded resources to match the current resolution. Rescales if needed.
    /// </summary>
    /// <param name="name">Name of the file to load, "sprites" or "random_mode". Not checked for validity because it's private code.</param>
    /// <param name="targetSpriteSize">Sprite size to rescale for, if needed.</param>
    /// <param name="resizeFunction">Function that resizes a given bitmap to match provided sprite size.
    /// Called to perform a rescale if the current resolution has no prescaled image variant.</param>
    /// <returns>Bitmap to use with current screen resolution.</returns>
    private SKBitmap LoadDefaultImage(string name, int targetSpriteSize, Func<SKBitmap, int, SKBitmap> resizeFunction)
    {
        var isSupported = SupportedResolutions.Contains(_currentResolution);
        var resolutionToUse = isSupported ? _currentResolution : SupportedResolutions.Max();
        var resName = $"Bnfour.MuseDashMods.ScoreboardCharacters.Resources.{name}.{resolutionToUse}.png";

        var assembly = GetType().GetTypeInfo().Assembly;
        using (var randomModeImageStream = assembly.GetManifestResourceStream(resName))
        {
            var bitmap = SKBitmap.Decode(randomModeImageStream);

            if (!isSupported)
            {
                bitmap = resizeFunction(bitmap, targetSpriteSize);
            }

            return bitmap;
        }
    }

    private SKBitmap RescaleSpritesheet(SKBitmap source, int targetSpriteSize)
    {
        // get number of rows from builtin image (premature optimization if it will be extended downwards for more character space),
        // assuming individual sprites are square
        var spriteRows = source.Height / (source.Width / Constants.SpritesPerRow);
        var scaledSize = new SKImageInfo(Constants.SpritesPerRow * targetSpriteSize, spriteRows * targetSpriteSize);

        return source.Resize(scaledSize, new SKSamplingOptions(SKCubicResampler.Mitchell));
    }
}
