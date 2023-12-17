using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities.ButtonImages
{
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

        public SpritesheetManager()
        {
            // use highest res available as fallback for non-supported resolution
            EmbeddedResourceFallbackName = string.Format(EmbeddedResourcePrescaledNameTemplate, SupportedResolutions.Max());
        }

        public SpritesheetSettings LoadSpritesheet()
        {
            _currentResolution = Screen.height;
            return LoadOverride() ?? LoadDefault();
        }

        public bool ReloadRequired()
        {
            return !_overrideActive && Screen.height != _currentResolution;
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
                var overrideBitmap = new Bitmap(overrideFullPath);
                if (overrideBitmap.Width % Constants.SpritesPerRow != 0)
                {
                    // TODO consider some kind of exception to indicate what exactly went wrong with override load -- 2023-12-14
                    // there should be 8 sprites per row
                    return null;
                }
                int potentialOverrideSpriteSize = overrideBitmap.Width / Constants.SpritesPerRow;
                if (overrideBitmap.Height % potentialOverrideSpriteSize != 0)
                {
                    // there should be an integer amount of rows
                    return null;
                }

                _overrideActive = true;
                return new SpritesheetSettings(overrideBitmap, potentialOverrideSpriteSize);
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

            var assembly = typeof(ButtonImageProvider).GetTypeInfo().Assembly;
            var defaultImageStream = assembly.GetManifestResourceStream(resName);
            var defaultBitmap = new Bitmap(defaultImageStream);
            if (!isSupported)
            {
                defaultBitmap = RescaleSpritesheet(defaultBitmap, spriteSize);
            }

            return new SpritesheetSettings(defaultBitmap, spriteSize);
        }

        private Bitmap RescaleSpritesheet(Bitmap source, int targetSpriteSize)
        {
            // get number of rows from builtin image (premature optimization if it will be extended downwards for more character space),
            // assuming individual sprites are square
            var spriteRows = source.Height / (source.Width / Constants.SpritesPerRow);
            
            var scaledBitmap = new Bitmap(Constants.SpritesPerRow * targetSpriteSize, spriteRows * targetSpriteSize);

            var sourceRect = new Rectangle(0, 0, source.Width, source.Height);
            var destRect = new Rectangle(0, 0, scaledBitmap.Width, scaledBitmap.Height);

            using (var graphics = System.Drawing.Graphics.FromImage(scaledBitmap))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(source, destRect, sourceRect, GraphicsUnit.Pixel);

                return scaledBitmap;
            }
        }
    }
}
