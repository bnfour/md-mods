using System;
using System.IO;
using System.Linq;
using System.Reflection;
using SkiaSharp;
using UnityEngine;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities.ButtonImages;

/// <summary>
/// Encapsulates loading (and reloading) spritesheets and related settings.
/// </summary>
public class SpritesheetManager
{
    private const string EmbeddedSpritesheetPrescaledNameTemplate = "Bnfour.MuseDashMods.ScoreboardCharacters.Resources.sprites.{0}.png";
    private const string EmbeddedRandomModeImagePrescaledNameTemplate = "Bnfour.MuseDashMods.ScoreboardCharacters.Resources.random_mode.{0}.png";
    private const string OverrideFilename = "scoreboard_characters_override.png";
    private readonly int[] SupportedResolutions = { 720, 1080, 1440, 2160 };
    // 1920×1080 is the baseline resolution
    private const int BaseResolution = 1080;
    private const int BaseSpriteSize = 40;

    private int _currentResolution;
    private bool _overrideActive;

    private bool _initialized;

    private readonly string _embeddedSpritesheetFallbackName;
    private readonly string _embeddedRandomModeImageFallbackName;

    private int CurrentSpriteSize => (int)Math.Round(BaseSpriteSize * (decimal)_currentResolution / BaseResolution, MidpointRounding.ToEven);

    public SpritesheetManager()
    {
        // use highest res available as fallback for non-supported resolution
        _embeddedSpritesheetFallbackName = string.Format(EmbeddedSpritesheetPrescaledNameTemplate, SupportedResolutions.Max());
        _embeddedRandomModeImageFallbackName = string.Format(EmbeddedRandomModeImagePrescaledNameTemplate, SupportedResolutions.Max());
    }

    public SpritesheetSettings LoadSpritesheet()
    {
        _initialized = true;
        _currentResolution = Screen.height;
        return LoadOverride() ?? LoadDefault();
    }

    public bool ReloadRequired()
    {
        return !_initialized && !_overrideActive && Screen.height != _currentResolution;
    }

    /// <summary>
    /// Tries to load the override image, if it contains square sprites with the same size, <see cref="Constants.SpritesPerRow"/> per row.
    /// Please note it only checks image dimensions, not the contents.
    /// </summary>
    /// <returns>
    /// Settings for the spritesheet if the override was successfully loaded,
    /// null if the override is not loaded for any reason (it doesn't exist or the resolution is wrong).
    /// </returns>
    private SpritesheetSettings LoadOverride()
    {
        var overrideFullPath = Path.Combine(Application.dataPath, OverrideFilename);
        if (File.Exists(overrideFullPath))
        {
            var overrideBitmap = SKImage.FromEncodedData(overrideFullPath);
            if (overrideBitmap.Width % Constants.SpritesPerRow != 0)
            {
                var logger = MelonLoader.Melon<ScoreboardCharactersMod>.Logger;
                logger.Warning($"The width of the override image ({overrideBitmap.Width}) is not divisible by {Constants.SpritesPerRow}. The override will not be applied.");
                return null;
            }
            int potentialOverrideSpriteSize = overrideBitmap.Width / Constants.SpritesPerRow;
            if (overrideBitmap.Height % potentialOverrideSpriteSize != 0)
            {
                var logger = MelonLoader.Melon<ScoreboardCharactersMod>.Logger;
                logger.Warning($"The height of the override image ({overrideBitmap.Height}) is not divisible by sprite size ({potentialOverrideSpriteSize}). The override will not be applied.");
                return null;
            }

            _overrideActive = true;
            return new SpritesheetSettings(SKBitmap.FromImage(overrideBitmap), LoadRandomModeImage(), potentialOverrideSpriteSize);
        }

        return null;
    }

    /// <summary>
    /// Creates settings instance that uses built-in spritesheet.
    /// If no pre-scaled version is available in the resources, on the fly rescale is performed.
    /// </summary>
    private SpritesheetSettings LoadDefault()
    {
        var isSupported = SupportedResolutions.Contains(_currentResolution);
        var resName = isSupported
            ? string.Format(EmbeddedSpritesheetPrescaledNameTemplate, _currentResolution)
            : _embeddedSpritesheetFallbackName;

        var assembly = GetType().GetTypeInfo().Assembly;

        using (var defaultImageStream = assembly.GetManifestResourceStream(resName))
        {
            var defaultBitmap = SKBitmap.Decode(defaultImageStream);

            if (!isSupported)
            {
                defaultBitmap = RescaleSpritesheet(defaultBitmap, CurrentSpriteSize);
            }

            return new SpritesheetSettings(defaultBitmap, LoadRandomModeImage(), CurrentSpriteSize);
        }
    }

    /// <summary>
    /// Loads the default random mode image for current resolution.
    /// If no pre-scaled version is available in the resources, on the fly rescale is performed.
    /// This image does not support overriding -- the built-in sprite is always used.
    /// </summary>
    private SKBitmap LoadRandomModeImage()
    {
        var isSupported = SupportedResolutions.Contains(_currentResolution);
        var resName = isSupported
            ? string.Format(EmbeddedRandomModeImagePrescaledNameTemplate, _currentResolution)
            : _embeddedRandomModeImageFallbackName;

        var assembly = GetType().GetTypeInfo().Assembly;

        using (var randomModeImageStream = assembly.GetManifestResourceStream(resName))
        {
            var bitmap = SKBitmap.Decode(randomModeImageStream);

            if (!isSupported)
            {
                bitmap = RescaleRandomModeImage(bitmap, CurrentSpriteSize);
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

        return source.Resize(scaledSize, SKFilterQuality.High);
    }

    private SKBitmap RescaleRandomModeImage(SKBitmap source, int targetSpriteSize)
    {
        // the aspect ratio is constant
        return source.Resize(new SKImageInfo(2 * targetSpriteSize, targetSpriteSize), SKFilterQuality.High);
    }
}
