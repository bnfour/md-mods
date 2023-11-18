using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using UnityEngine;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities
{
    /// <summary>
    /// Generates images for the custom buttons from the built-in spritesheet or file-based override.
    /// Has an internal image cache that lasts until playing a level.
    /// </summary>
    public class ButtonImageProvider
    {
        private const string EmbeddedResourceName = "Bnfour.MuseDashMods.ScoreboardCharacters.Resources.sprites.png";
        private const string OverrideFilename = "scoreboard_characters_override.png";

        private readonly Bitmap CustomAtlas;

        private readonly Dictionary<(Character, Elfin), Sprite> Cache = new Dictionary<(Character, Elfin), Sprite>();

        private const int CharactersPerRow = 5;
        private const int ElfinsPerRow = 3;
        private const int ElfinStartColumn = 5;

        private const int SpriteSize = 120;
        // the texture's dimensions should be powers of two to avoid mipmapping artifacts
        private const int TextureWidth = 256;
        private const int TextureHeight = 128;

        private readonly Rectangle CharacterDestinationRectangle = new Rectangle(0, 0, SpriteSize, SpriteSize);
        private readonly Rectangle ElfinDestinationRectangle = new Rectangle(SpriteSize, 0, SpriteSize, SpriteSize);

        public ButtonImageProvider()
        {
            var assembly = typeof(ButtonImageProvider).GetTypeInfo().Assembly;
            var defaultImageStream = assembly.GetManifestResourceStream(EmbeddedResourceName);
            var defaultBitmap = new Bitmap(defaultImageStream);

            var overrideActive = false;

            var overrideFullPath = Path.Combine(Application.dataPath, OverrideFilename);
            if (File.Exists(overrideFullPath))
            {
                var overrideBitmap = new Bitmap(overrideFullPath);
                if (overrideBitmap.Width == defaultBitmap.Width
                    && overrideBitmap.Height == defaultBitmap.Height)
                {
                    overrideActive = true;
                    CustomAtlas = overrideBitmap;
                }
            }

            if (!overrideActive)
            {
                CustomAtlas = defaultBitmap;
            }
        }

        public Sprite GetSprite(Character character, Elfin elfin)
        {
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
                    var texture = new Texture2D(1, 1);
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
