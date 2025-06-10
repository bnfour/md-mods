using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SkiaSharp;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities.ButtonImages;

/// <summary>
/// Generates images for the custom buttons from the built-in spritesheet or file-based override.
/// Has an internal image cache that lasts until playing a level (or resolution change).
/// </summary>
public class ButtonImageProvider
{
    private readonly Dictionary<(Character, Elfin), Sprite> _cache = new();

    private Sprite _randomModeSprite;

    private readonly SpritesheetManager _manager = new();

    private SpritesheetSettings _settings;

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

    public Sprite GetRandomSprite()
    {
        // TODO actual sprite
        return _randomModeSprite ??= CreateRandomModeSprite();
    }

    public void ResetCache()
    {
        _cache.Clear();
        _randomModeSprite = null;
    }

    private Sprite CreateSprite(Character character, Elfin elfin)
    {
        var size = _settings.SpriteSize;
        var bitmap = new SKBitmap(2 * size, size);
        using (var canvas = new SKCanvas(bitmap))
        {
            canvas.Clear();
            canvas.DrawBitmap(_settings.Bitmap, GetSpriteRectangle(character), _settings.CharacterDest);
            canvas.DrawBitmap(_settings.Bitmap, GetSpriteRectangle(elfin), _settings.ElfinDest);
            canvas.Flush();
            // TODO consider reusing sprite creation here and in CreateRandomModeSprite
            using (var data = bitmap.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = new MemoryStream())
            {
                data.SaveTo(stream);
                // texture size here is irrelevant as it gets changed by LoadImage,
                // mipmap is off as we supply pre-scaled images and do not want any unity scaling involved
                var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                ImageConversion.LoadImage(texture, stream.ToArray());
                // (UnityEngine.)Rect is not a (SkiaSharp.)SKRect(I), unfortunate mixing in one file
                var sprite = Sprite.Create(texture, new Rect(0, 0, 2 * size, size), new Vector2(0.5f, 0.5f));

                return sprite;
            }
        }
    }

    private Sprite CreateRandomModeSprite()
    {
        using (var data = _settings.RandomButtonBitmap.Encode(SKEncodedImageFormat.Png, 100))
        using (var stream = new MemoryStream())
        {
            data.SaveTo(stream);
            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            ImageConversion.LoadImage(texture, stream.ToArray());

            return Sprite.Create(texture, new Rect(0, 0, 2 * _settings.SpriteSize, _settings.SpriteSize), new Vector2(0.5f, 0.5f));
        }
    }

    private SKRectI GetSpriteRectangle(Character character)
    {
        var spriteIndex = (int)character;
        var size = _settings.SpriteSize;

        var columnIndex = spriteIndex % Constants.CharactersPerRow;
        var rowIndex = spriteIndex / Constants.CharactersPerRow;

        var x = columnIndex * size;
        var y = rowIndex * size;

        return new SKRectI(x, y, x + size, y + size);
    }

    private SKRectI GetSpriteRectangle(Elfin elfin)
    {
        // elfins start from -1
        var spriteIndex = (int)elfin + 1;
        var size = _settings.SpriteSize;

        var columnIndex = Constants.ElfinStartColumn + (spriteIndex % Constants.ElfinsPerRow);
        var rowIndex = spriteIndex / Constants.ElfinsPerRow;

        var x = columnIndex * size;
        var y = rowIndex * size;

        return new SKRectI(x, y, x + size, y + size);
    }
}
