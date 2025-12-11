using System.Reflection;
using UnityEngine;

namespace Bnfour.MuseDashMods.FeverSwitch.Utilities;

internal class SpriteProvider
{
    // "key" is toggle state, true = on

    internal Sprite Off => GetSprite(false);
    internal Sprite On => GetSprite(true);

    // holds raw image data that is loaded once
    // it's like 10 KiB total for both
    private readonly Dictionary<bool, byte[]> _toggleIcons;

    private readonly Dictionary<bool, Sprite> _cache;

    internal SpriteProvider(bool isAutoDefault)
    {
        _toggleIcons = [];
        _cache = [];

        var assembly = GetType().Assembly;
        // TODO looks scuffed, maybe switch to an enum?
        foreach (var k in new[] { false, true })
        {
            _toggleIcons[k] = LoadRawImage(k, isAutoDefault, assembly);
        }
    }

    internal void ResetCache()
    {
        _cache.Clear();
    }

    private static byte[] LoadRawImage(bool key, bool isAutoDefault, Assembly assembly)
    {
        var kind = key ? "accented" : "default";
        // TODO why's that, find a way to format the table?
        var name = key ^ isAutoDefault ? "auto" : "manual";

        var resName = $"Bnfour.MuseDashMods.FeverSwitch.Resources.{name}.{kind}.png";

        using (var resStream = assembly.GetManifestResourceStream(resName))
        using (MemoryStream memoryStream = new())
        {
            if (resStream != null)
            {
                resStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
            throw new ApplicationException($"Unable to load {resName} from assembly");
        }
    }

    private static Sprite CreateSpriteFromPNGData(byte[] data)
    {
        // TODO unironically consider mipmap for the medal sprites, they're 128×128
        var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        ImageConversion.LoadImage(texture, data);

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    
    private Sprite GetSprite(bool key)
    {
        if (_cache.ContainsKey(key))
        {
            return _cache[key];
        }

        var sprite = CreateSpriteFromPNGData(_toggleIcons[key]);
        _cache[key] = sprite;

        return sprite;
    }
}
