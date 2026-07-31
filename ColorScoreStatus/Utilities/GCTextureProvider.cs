using UnityEngine;

using Bnfour.MuseDashMods.ColorScoreStatus.Data;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Utilities;

internal static class GCTextureProvider
{
    private const int TextureHeight = 32;

    internal static Texture2D CreateTexture(ComboStatus status)
    {
        var palette = Palette.ForStatus(status);

        var texture = new Texture2D(1, TextureHeight, TextureFormat.RGB24, false);
        for (int i = 0; i < TextureHeight; i++)
        {
            // no idea which way is up, this creates a gradient that is shown correctly
            var pixelColor = Color.Lerp(palette.Light, palette.Main, (float)i / (TextureHeight - 1));
            texture.SetPixel(0, i, pixelColor);
        }
        texture.Apply();

        return texture;
    }
}
