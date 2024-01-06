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
    private const string EmbeddedResourcePrescaledNameTemplate = "Bnfour.MuseDashMods.ScoreboardCharacters.Resources.sprites.{0}.png";
    private const string OverrideFilename = "scoreboard_characters_override.png";
    private readonly string EmbeddedResourceFallbackName;

    private readonly int[] SupportedResolutions = { 720, 1080, 1440, 2160 };
    // 1920×1080 is the baseline resolution
    private const int BaseResolution = 1080;
    private const int BaseSpriteSize = 40;

    private int _currentResolution;
    private bool _overrideActive;

    private bool _initialized;

    public SpritesheetManager()
    {
        // use highest res available as fallback for non-supported resolution
        EmbeddedResourceFallbackName = string.Format(EmbeddedResourcePrescaledNameTemplate, SupportedResolutions.Max());
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
            return new SpritesheetSettings(SKBitmap.FromImage(overrideBitmap), potentialOverrideSpriteSize);
        }

        return null;
    }

    /// <summary>
    /// Loads the default spritesheet for current resolution.
    /// If no pre-scaled version is available in the resources, on the fly rescale is performed.
    /// </summary>
    private SpritesheetSettings LoadDefault()
    {
        int spriteSize = (int)Math.Round(BaseSpriteSize * (decimal)_currentResolution / BaseResolution, MidpointRounding.ToEven);

        var isSupported = SupportedResolutions.Contains(_currentResolution);
        var resName = isSupported
            ? string.Format(EmbeddedResourcePrescaledNameTemplate, _currentResolution)
            : EmbeddedResourceFallbackName;

        var assembly = GetType().GetTypeInfo().Assembly;

        using (var defaultImageStream = assembly.GetManifestResourceStream(resName))
        {
            var defaultBitmap = SKBitmap.Decode(defaultImageStream);

            if (!isSupported)
            {
                defaultBitmap = RescaleSpritesheet(defaultBitmap, spriteSize);
            }

            return new SpritesheetSettings(defaultBitmap, spriteSize);
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
}
