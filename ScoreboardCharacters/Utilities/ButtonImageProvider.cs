using System;
using System.Collections.Generic;
using Bnfour.MuseDashMods.ScoreboardCharacters.Data;
using UnityEngine;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities
{
    public static class ButtonImageProvider
    {
        // very WIP, excuse the mess
        private const string HardcodedAtlasFilename = "testimage.png";

        private static readonly Dictionary<(Character, Elfin), Sprite> Cache = new Dictionary<(Character, Elfin), Sprite>();

        private const int CharactersPerRow = 5;
        private const int ElfinsPerRow = 3;
        private const int ElfinStartColumn = 5;
        // 3× the supposed size
        private const int ImageSize = 120;

        public static Sprite GetSprite(Character character, Elfin elfin)
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

        private static Sprite CreateSprite(Character character, Elfin elfin)
        {
            // get character part, get elfin part, mash them together somehow
            // dunno if unity itself can do this
            throw new NotImplementedException("soon™");
            // let's assume we did this, and rawImageData contains bytes of the result
            byte[] rawImageData = new byte[1];
            // texture size here is irrelevant as it gets changed by LoadImage
            var texture = new Texture2D(1, 1);
            ImageConversion.LoadImage(texture, rawImageData);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 2 * ImageSize, ImageSize), new Vector2(0.5f, 0.5f));

            return sprite;
        }

        private static (int x, int y) GetSpriteTopLeft(Character character)
        {
            var spriteIndex = (int)character;

            var columnIndex = spriteIndex % CharactersPerRow;
            var rowIndex = spriteIndex / CharactersPerRow;

            return (columnIndex * ImageSize, rowIndex * ImageSize);
        }

        private static (int x, int y) GetSpriteTopLeft(Elfin elfin)
        {
            // elfins start from -1
            var spriteIndex = (int)elfin + 1;

            var columnIndex = spriteIndex % ElfinsPerRow;
            var rowIndex = spriteIndex / ElfinsPerRow;

            return ((ElfinStartColumn + columnIndex) * ImageSize, rowIndex * ImageSize);
        }
    }
}
