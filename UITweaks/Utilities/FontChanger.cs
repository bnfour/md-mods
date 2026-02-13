using System.IO;
using System.Reflection;

using UnityEngine;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.UITweaks.Utilities;

/// <summary>
/// Replaces the font for some numbers in the scoreboard entries with a custom one
/// loaded from a custom asset bundle embedded into the mod assembly.
/// </summary>
internal class FontChanger
{
    private const string AssetBundleResourceName = "Bnfour.MuseDashMods.UITweaks.Resources.tekomononumbers";
    private const string FontPath = "Assets/fonts/Teko-Bold-mono-numbers-take-4.ttf";
    private static readonly string[] ComponentsToModify =
    [
        "TxtRankValueS",
        "TxtScoreValueS",
        "TxtAccuracyValueS"
    ];

    private Font _customFont;

    internal Font CustomFont => _customFont ??= LoadFont();

    internal void ChangeNumericFonts(GameObject rankLine)
    {
        foreach (var id in ComponentsToModify)
        {
            var scoreText = rankLine.transform.Find(id).GetComponent<Text>();
            scoreText.font = CustomFont;
        }
    }

    private Font LoadFont()
    {
        var assembly = GetType().GetTypeInfo().Assembly;
        // the usual embedded resource -> byte array conversion
        // TODO consider async
        using (var bundleStream = assembly.GetManifestResourceStream(AssetBundleResourceName))
        using (MemoryStream memoryStream = new())
        {
            bundleStream.CopyTo(memoryStream);
            var bundle = AssetBundle.LoadFromMemory(memoryStream.ToArray());
            var font = bundle?.LoadAsset<Font>(FontPath);
            // we got the font we wanted and will store it momentarily,
            // the bundle itself is no longer needed
            bundle?.Unload(false);

            return font;
        }
    }
}
