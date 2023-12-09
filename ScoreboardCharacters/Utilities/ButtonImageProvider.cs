using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using UnityEngine;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;
using System;
using System.Linq;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities
{
    /// <summary>
    /// Generates images for the custom buttons from the built-in spritesheet or file-based override.
    /// Has an internal image cache that lasts until playing a level.
    /// </summary>
    public class ButtonImageProvider
    {
        private const string EmbeddedResourcePrescaledNameTemplate = "Bnfour.MuseDashMods.ScoreboardCharacters.Resources.sprites.{0}.png";
        // TODO remove, use closest/highest res prescale
        private const string EmbeddedResourceFallbackName = "Bnfour.MuseDashMods.ScoreboardCharacters.Resources.sprites.png";
        // private const string OverrideFilename = "scoreboard_characters_override.png";

        private readonly int[] SupportedResolutions = { 1080, 1440 };

        private const int BaseSpriteSize = 40;
        private const int BaseResolution = 1080;

        private Bitmap CustomAtlas;

        private readonly Dictionary<(Character, Elfin), Sprite> Cache = new Dictionary<(Character, Elfin), Sprite>();

        private const int CharactersPerRow = 5;
        private const int ElfinsPerRow = 3;
        private const int ElfinStartColumn = 5;


        // the texture's dimensions should be powers of two to avoid mipmapping artifacts
        private const int TextureWidth = 256;
        private const int TextureHeight = 128;

        private int SpriteSize;
        private Rectangle CharacterDestinationRectangle;// = new Rectangle(0, 0, SpriteSize, SpriteSize);
        private Rectangle ElfinDestinationRectangle;// = new Rectangle(SpriteSize, 0, SpriteSize, SpriteSize);
        private int CurrentResolution;

        public ButtonImageProvider()
        {
            ConfigureScaling();
        }

        public Sprite GetSprite(Character character, Elfin elfin)
        {
            ConfigureScaling();

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

        private void ConfigureScaling()
        {
            var screenHeight = Screen.height;
            if (CurrentResolution == screenHeight)
            {
                return;
            }

            CurrentResolution = screenHeight;

            var isSupported = SupportedResolutions.Contains(screenHeight);

            SpriteSize = (int)Math.Round(BaseSpriteSize * (decimal)screenHeight / BaseResolution, MidpointRounding.ToEven);
            CharacterDestinationRectangle = new Rectangle(0, 0, SpriteSize, SpriteSize);
            ElfinDestinationRectangle = new Rectangle(SpriteSize, 0, SpriteSize, SpriteSize);

            var resName = isSupported
                ? string.Format(EmbeddedResourcePrescaledNameTemplate, screenHeight)
                : EmbeddedResourceFallbackName;

            var assembly = typeof(ButtonImageProvider).GetTypeInfo().Assembly;
            var defaultImageStream = assembly.GetManifestResourceStream(resName);
            var defaultBitmap = new Bitmap(defaultImageStream);

            var overrideActive = false;
            // TODO overriding here?
            // skip the method alltogether if already overriden and no scaling?
            if (!overrideActive)
            {
                CustomAtlas = defaultBitmap;
            }
            // clear the cache in case resolution was switched at runtime
            // as this is pretty mush the way for me to test it
            ResetCache();
        }

        private Sprite CreateSprite(Character character, Elfin elfin)
        {
            var buttonBitmap = new Bitmap(TextureWidth, TextureHeight);
            using (var graphics = System.Drawing.Graphics.FromImage(buttonBitmap))
            {
                graphics.DrawImage(CustomAtlas, CharacterDestinationRectangle, GetSpriteRectangle(character), GraphicsUnit.Pixel);
                graphics.DrawImage(CustomAtlas, ElfinDestinationRectangle, GetSpriteRectangle(elfin), GraphicsUnit.Pixel);
                using (var byteStream = new MemoryStream())
                {
                    buttonBitmap.Save(byteStream, System.Drawing.Imaging.ImageFormat.Png);
                    // texture size here is irrelevant as it gets changed by LoadImage
                    var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    ImageConversion.LoadImage(texture, byteStream.ToArray());
                    // Rect is not a Rectangle, unfortunate mixing in one file
                    // it seems that vertical positioning for those is also different,
                    // so we need to adjust the coordinates to crop
                    var sprite = Sprite.Create(texture, new Rect(0, TextureHeight - SpriteSize, 2 * SpriteSize, SpriteSize), new Vector2(0.5f, 0.5f));

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
