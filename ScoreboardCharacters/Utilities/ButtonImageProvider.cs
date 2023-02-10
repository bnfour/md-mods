using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Bnfour.MuseDashMods.ScoreboardCharacters.Data;
using UnityEngine;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities
{
    public class ButtonImageProvider
    {
        // very WIP, excuse the mess
        private const string HardcodedAtlasFilename = "testimage.png";

        // TODO make file loading an optional override, provide a default atlas
        private readonly Bitmap CustomAtlas = new Bitmap(Path.Combine(Application.dataPath, HardcodedAtlasFilename));

        private readonly Dictionary<(Character, Elfin), Sprite> Cache = new Dictionary<(Character, Elfin), Sprite>();

        private const int CharactersPerRow = 5;
        private const int ElfinsPerRow = 3;
        private const int ElfinStartColumn = 5;
        // 3× the supposed size
        // TODO support for non-square sprites?
        private const int SpriteSize = 120;

        private readonly Rectangle CharacterDestinationRectangle = new Rectangle(0, 0, SpriteSize, SpriteSize);
        private readonly Rectangle ElfinDestinationRectangle = new Rectangle(SpriteSize, 0, SpriteSize, SpriteSize);

        // TODO pre-cache combos popular on the scoreboard, such as 11/7, 3/6, 3/5, 7/6?
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
            var buttonBitmap = new Bitmap(2 * SpriteSize, SpriteSize);
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
