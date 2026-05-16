using System.Collections.Generic;
using System.IO;

using UnityEngine;

using Bnfour.MuseDashMods.ColorScoreStatus.Data;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Utilities;

internal static class GCTextureProvider
{
    private const string ResourcePathTemplate = "Bnfour.MuseDashMods.ColorScoreStatus.Resources.GCScore.{0}.png";

    private static readonly Dictionary<ComboStatus, byte[]> _rawPngData = new();

    static GCTextureProvider()
    {
        foreach (var (status, name) in new[] { (ComboStatus.AllPerfect, "Gold"), (ComboStatus.FullCombo, "Silver"), (ComboStatus.ThereWasAnAttempt, "Red") })
        {
            _rawPngData[status] = LoadRawTexture(string.Format(ResourcePathTemplate, name));
        }
    }

    internal static Texture CreateTexture(ComboStatus status)
    {
        var texture = new Texture2D(1, 1, TextureFormat.RGB24, false);
        ImageConversion.LoadImage(texture, _rawPngData[status]);

        return texture;
    }

    private static byte[] LoadRawTexture(string resourcePath)
    {
        var assembly = typeof(GCTextureProvider).Assembly;

        using (var resStream = assembly.GetManifestResourceStream(resourcePath))
        using (MemoryStream memoryStream = new())
        {
            if (resStream != null)
            {
                resStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
            throw new System.ApplicationException($"Unable to load {resourcePath} from assembly");
        }
    }
}
