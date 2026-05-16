using Il2CppTMPro;
using System;
using System.Collections.Generic;
using System.Linq;
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
    private readonly Dictionary<ComboStatus, Texture> _replacements
        = Enum.GetValues<ComboStatus>()
        .ToDictionary(e => e, GCTextureProvider.CreateTexture);


    public override void SetStateTo(ComboStatus status)
    {
        var scoreFontMaterial = _reference.GetComponent<TextMeshProUGUI>()?.font?.material;

        scoreFontMaterial?.SetTexture("_FaceTex", _replacements[status]);
        scoreFontMaterial?.SetColor("_GlowColor", ForStatus(status).GCGlow);
    }
}
