using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

using Bnfour.MuseDashMods.FeverSwitch.Data;

namespace Bnfour.MuseDashMods.FeverSwitch.Utilities;

/// <summary>
/// Stores the raw PNG data for custom images, creates Sprites from them on demand,
/// caching the results.
/// </summary>
internal class SpriteProvider
{
    private const string CommonPathPrefix = "Bnfour.MuseDashMods.FeverSwitch.Resources";

    internal Sprite Off => GetSprite(SpriteKind.ToggleOff);
    internal Sprite On => GetSprite(SpriteKind.ToggleOn);
    internal Sprite Hint => GetSprite(SpriteKind.Hint);

    // raw image data that is loaded once
    // it's like 10 KiB total
    private readonly Dictionary<SpriteKind, byte[]> _toggleIcons;

    private readonly Dictionary<SpriteKind, Sprite> _cache;

    internal SpriteProvider(bool isAutoDefault)
    {
        _toggleIcons = [];
        _cache = [];

        var assembly = GetType().Assembly;
        // TODO still looks scuffed
        foreach ((SpriteKind kind, bool isAccented) in new[] { (SpriteKind.ToggleOff, false), (SpriteKind.ToggleOn, true) })
        {
            var imageKind = isAccented ? "accented" : "default";
            // accented image is the one that is not default, so we can use xor here
            var imageName = isAccented ^ isAutoDefault ? "auto" : "manual";

            var resName = $"{CommonPathPrefix}.{imageName}.{imageKind}.png";

            _toggleIcons[kind] = LoadRawImage(resName, assembly);
        }

        _toggleIcons[SpriteKind.Hint] = LoadRawImage($"{CommonPathPrefix}.PcButtonF.png", assembly);
    }

    internal void ResetCache()
    {
        _cache.Clear();
    }

    private static byte[] LoadRawImage(string resName, Assembly assembly)
    {
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
        // TODO unironically consider mipmap for:
        // - the medal sprites, they're 128×128
        // - F hint, it's 64×64
        var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        ImageConversion.LoadImage(texture, data);

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private Sprite GetSprite(SpriteKind key)
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
