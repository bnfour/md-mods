using Il2CppTMPro;
using UnityEngine;

using Bnfour.MuseDashMods.ColorScoreStatus.Data;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Utilities.ColorSetting;

/// <summary>
/// Utility class to change colors for Groove Coaster UI's score, implemented as
/// a TextMeshPro with a lot of effects.
/// </summary>
/// <param name="gc">GameObject containing the score component.</param>
internal class GCScoreColorer(GameObject gc) : ScoreColorerBase(gc), IScoreColorer
{
    public override void SetStateTo(ComboStatus status)
    {
        var scoreFontMaterial = _reference.GetComponent<TextMeshProUGUI>()?.font?.material;

        scoreFontMaterial?.SetTexture("_FaceTex", GCTextureProvider.CreateTexture(status));
        scoreFontMaterial?.SetColor("_GlowColor", ForStatus(status).GCGlow);
    }
}
