using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UnityEngine;

using Graphics = System.Drawing.Graphics;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities.ButtonImages
{
    /// <summary>
    /// Generates images for the custom buttons from the built-in spritesheet or file-based override.
    /// Has an internal image cache that lasts until playing a level (or resolution change).
    /// </summary>
    public class ButtonImageProvider
    {
        private readonly Dictionary<(Character, Elfin), Sprite> _cache = new Dictionary<(Character, Elfin), Sprite>();

        private readonly SpritesheetManager _manager = new SpritesheetManager();

        private SpritesheetSettings _settings;

        public ButtonImageProvider()
        {
            _settings = _manager.LoadSpritesheet();
        }

        public Sprite GetSprite(Character character, Elfin elfin)
        {
            if (_manager.ReloadRequired())
            {
                _settings = _manager.LoadSpritesheet();
                ResetCache();
            }

            var keyTuple = (character, elfin);

            if (_cache.ContainsKey(keyTuple))
            {
                return _cache[keyTuple];
            }

            var newSprite = CreateSprite(character, elfin);
            _cache[keyTuple] = newSprite;
            return newSprite;
        }

        public void ResetCache()
        {
            _cache.Clear();
        }

        private Sprite CreateSprite(Character character, Elfin elfin)
        {
            var size = _settings.SpriteSize;
            var buttonBitmap = new Bitmap(2 * size, size);
            using (var graphics = Graphics.FromImage(buttonBitmap))
            {
                graphics.DrawImage(_settings.Bitmap, _settings.CharacterDest, GetSpriteRectangle(character), GraphicsUnit.Pixel);
                graphics.DrawImage(_settings.Bitmap, _settings.ElfinDest, GetSpriteRectangle(elfin), GraphicsUnit.Pixel);
                using (var byteStream = new MemoryStream())
                {
                    buttonBitmap.Save(byteStream, ImageFormat.Png);
                    // texture size here is irrelevant as it gets changed by LoadImage,
                    // mipmap is off as we supply pre-scaled images and do not want any unity scaling involved
                    var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    ImageConversion.LoadImage(texture, byteStream.ToArray());
                    // (UnityEngine.)Rect is not a (System.Drawing.)Rectangle, unfortunate mixing in one file
                    // just for reference, their vertical axis seem to go to different directions, it was an issue earlier (see #8)
                    var sprite = Sprite.Create(texture, new Rect(0, 0, 2 * size, size), new Vector2(0.5f, 0.5f));

                    return sprite;
                }
            }
        }

        private Rectangle GetSpriteRectangle(Character character)
        {
            var spriteIndex = (int)character;
            var size = _settings.SpriteSize;

            var columnIndex = spriteIndex % Constants.CharactersPerRow;
            var rowIndex = spriteIndex / Constants.CharactersPerRow;

            return new Rectangle(columnIndex * size, rowIndex * size, size, size);
        }

        private Rectangle GetSpriteRectangle(Elfin elfin)
        {
            // elfins start from -1
            var spriteIndex = (int)elfin + 1;
            var size = _settings.SpriteSize;

            var columnIndex = spriteIndex % Constants.ElfinsPerRow;
            var rowIndex = spriteIndex / Constants.ElfinsPerRow;

            return new Rectangle((Constants.ElfinStartColumn + columnIndex) * size, rowIndex * size, size, size);
        }
    }
}
