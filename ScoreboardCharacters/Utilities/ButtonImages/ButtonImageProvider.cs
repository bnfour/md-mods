using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities.ButtonImages
{
    /// <summary>
    /// Generates images for the custom buttons from the built-in spritesheet or file-based override.
    /// Has an internal image cache that lasts until playing a level.
    /// </summary>
    public class ButtonImageProvider
    {
        // please note that this class only cares about vertical resolution,
        // so any mention of "resolution" there is a single number of screen height

        // TODO move constants and some fields to other classes

        private const string EmbeddedResourcePrescaledNameTemplate = "Bnfour.MuseDashMods.ScoreboardCharacters.Resources.sprites.{0}.png";

        private const string OverrideFilename = "scoreboard_characters_override.png";

        private readonly int[] SupportedResolutions = { 720, 1080, 1440, 2160 };

        // using 1920×1080 as baseline resolution
        private const int BaseSpriteSize = 40;
        private const int BaseResolution = 1080;

        private readonly string EmbeddedResourceFallbackName;

        private Bitmap CustomAtlas;

        private readonly Dictionary<(Character, Elfin), Sprite> Cache = new Dictionary<(Character, Elfin), Sprite>();

        private const int CharactersPerRow = 5;
        private const int ElfinsPerRow = 3;

        private const int SpritesPerRow = CharactersPerRow + ElfinsPerRow;
        private const int ElfinStartColumn = 5;

        private int SpriteSize;
        private Rectangle CharacterDestinationRectangle;
        private Rectangle ElfinDestinationRectangle;
        private int CurrentResolution;

        private bool OverrideActive;

        public ButtonImageProvider()
        {
            // use highest res available as fallback for non-supported resolution
            EmbeddedResourceFallbackName = string.Format(EmbeddedResourcePrescaledNameTemplate, SupportedResolutions.Max());

            ConfigureScalingIfNeeded();
        }

        public Sprite GetSprite(Character character, Elfin elfin)
        {
            ConfigureScalingIfNeeded();

            var keyTuple = (character, elfin);

            if (Cache.ContainsKey(keyTuple))
            {
                return Cache[keyTuple];
            }

            var newSprite = CreateSprite(character, elfin);
            Cache[keyTuple] = newSprite;
            return newSprite;
        }

        public void ResetCache()
        {
            Cache.Clear();
        }

        private void ConfigureScalingIfNeeded()
        {
            // do nothing if resulotion is current or the image override is in effect
            var screenHeight = Screen.height;
            if (CurrentResolution == screenHeight || OverrideActive)
            {
                return;
            }

            CurrentResolution = screenHeight;

            if (!TryLoadOverride())
            {
                LoadDefaultSpritesheet(CurrentResolution);
            }

            // clear the cache in case resolution was switched at runtime
            ResetCache();
        }

        /// <summary>
        /// Loads the default spritesheet for a given resolution.
        /// If no pre-scaled version is available in the resources, on the fly rescale is performed.
        /// </summary>
        /// <param name="resolution">Target vertical resolution.</param>
        private void LoadDefaultSpritesheet(int resolution)
        {
            SpriteSize = (int)Math.Round(BaseSpriteSize * (decimal)resolution / BaseResolution, MidpointRounding.ToEven);
            CharacterDestinationRectangle = new Rectangle(0, 0, SpriteSize, SpriteSize);
            ElfinDestinationRectangle = new Rectangle(SpriteSize, 0, SpriteSize, SpriteSize);

            var isSupported = SupportedResolutions.Contains(resolution);
            var resName = isSupported
                ? string.Format(EmbeddedResourcePrescaledNameTemplate, resolution)
                : EmbeddedResourceFallbackName;

            var assembly = typeof(ButtonImageProvider).GetTypeInfo().Assembly;
            var defaultImageStream = assembly.GetManifestResourceStream(resName);
            var defaultBitmap = new Bitmap(defaultImageStream);

            if (!isSupported)
            {
                defaultBitmap = RescaleSpritesheet(defaultBitmap, SpriteSize);
            }

            CustomAtlas = defaultBitmap;
        }

        /// <summary>
        /// Tries to load the override image, and sets it as the sprite source
        /// if it contains square sprites with the same size, <see cref="SpritesPerRow"/> per row.
        /// Please note it only checks image dimensions, not the contents.
        /// </summary>
        /// <returns>True if the override was successfully loaded, false otherwise.</returns>
        private bool TryLoadOverride()
        {
            var overrideFullPath = Path.Combine(Application.dataPath, OverrideFilename);
            if (File.Exists(overrideFullPath))
            {
                var overrideBitmap = new Bitmap(overrideFullPath);
                
                if (overrideBitmap.Width % SpritesPerRow != 0)
                {
                    // TODO consider some kind of exception to indicate what exactly went wrong with override load -- 2023-12-14
                    // there should be 8 sprites per row
                    return false;
                }
                int potentialOverrideSpriteSize = overrideBitmap.Width / SpritesPerRow;
                if (overrideBitmap.Height % potentialOverrideSpriteSize != 0)
                {
                    // there should be integer amount of rows
                    return false;
                }

                SpriteSize = potentialOverrideSpriteSize;
                CharacterDestinationRectangle = new Rectangle(0, 0, SpriteSize, SpriteSize);
                ElfinDestinationRectangle = new Rectangle(SpriteSize, 0, SpriteSize, SpriteSize);

                OverrideActive = true;
                CustomAtlas = overrideBitmap;

                return true;
            }

            return false;
        }

        private Bitmap RescaleSpritesheet(Bitmap source, int targetSpriteSize)
        {
            // get number of rows from builtin image (premature optimization if it will be extended downwards for more character space),
            // assuming individual sprites are square
            var spriteRows = source.Height / (source.Width / SpritesPerRow);
            
            var scaledBitmap = new Bitmap(SpritesPerRow * targetSpriteSize, spriteRows * targetSpriteSize);

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

        private Sprite CreateSprite(Character character, Elfin elfin)
        {
            var buttonBitmap = new Bitmap(2 * SpriteSize, SpriteSize);
            using (var graphics = System.Drawing.Graphics.FromImage(buttonBitmap))
            {
                graphics.DrawImage(CustomAtlas, CharacterDestinationRectangle, GetSpriteRectangle(character), GraphicsUnit.Pixel);
                graphics.DrawImage(CustomAtlas, ElfinDestinationRectangle, GetSpriteRectangle(elfin), GraphicsUnit.Pixel);
                using (var byteStream = new MemoryStream())
                {
                    buttonBitmap.Save(byteStream, ImageFormat.Png);
                    // texture size here is irrelevant as it gets changed by LoadImage,
                    // also turning mipmapping off
                    var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    ImageConversion.LoadImage(texture, byteStream.ToArray());
                    // Rect is not a Rectangle, unfortunate mixing in one file
                    var sprite = Sprite.Create(texture, new Rect(0, 0, 2 * SpriteSize, SpriteSize), new Vector2(0.5f, 0.5f));

                    return sprite;
                }
            }
        }

        private Rectangle GetSpriteRectangle(Character character)
        {
            var spriteIndex = (int)character;

            var columnIndex = spriteIndex % CharactersPerRow;
            var rowIndex = spriteIndex / CharactersPerRow;

            return new Rectangle(columnIndex * SpriteSize, rowIndex * SpriteSize, SpriteSize, SpriteSize);
        }

        private Rectangle GetSpriteRectangle(Elfin elfin)
        {
            // elfins start from -1
            var spriteIndex = (int)elfin + 1;

            var columnIndex = spriteIndex % ElfinsPerRow;
            var rowIndex = spriteIndex / ElfinsPerRow;

            return new Rectangle((ElfinStartColumn + columnIndex) * SpriteSize, rowIndex * SpriteSize, SpriteSize, SpriteSize);
        }
    }
}
