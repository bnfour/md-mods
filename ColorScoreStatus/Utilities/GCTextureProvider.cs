using UnityEngine;

using Bnfour.MuseDashMods.ColorScoreStatus.Data;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Utilities;

internal static class GCTextureProvider
{
    private const int TextureHeight = 32;

    internal static Texture2D CreateTexture(ComboStatus status)
    {
        var texture = new Texture2D(1, TextureHeight, TextureFormat.RGB24, false);

        for (int i = 0; i < TextureHeight; i++)
        {
            // TODO actual gradient
            texture.SetPixel(0, i, i % 2 == 0 ? Color.black : Color.magenta);
        }
        texture.Apply();

        return texture;
    }
}
