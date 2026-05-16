using Il2CppTMPro;
using UnityEngine;

using Bnfour.MuseDashMods.ColorScoreStatus.Data;
using MelonLoader;

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
        // TODO check if the score (as opposed to combo i've prototyped on) has:
        // - texture
        // - colors set as material properties (glow, outline)
        // and actually implement the coloring
        var score = _reference.GetComponent<TextMeshProUGUI>();
        var logger = Melon<ColorScoreStatusMod>.Logger;
        logger.Msg($"just pretend the status was visibly set to {status} LULE");

        logger.Msg($"text color: {score.color.r}, {score.color.g}, {score.color.b}");
        // 1 1 1 — just white for the texture i guess

        logger.Msg($"text gradient: {score.colorGradient}");
        // some struct probably ignored in favor of the texture

        logger.Msg($"text outline: {score.outlineColor.r}, {score.outlineColor.g}, {score.outlineColor.b} (width {score.outlineWidth})");
        // #ffffff, but width 0, not shown?

        logger.Msg($"text face texture {score.font?.material.GetTexture("_FaceTex")?.name}");
        // gradient as a 32×32 texture, ScoreTexGC

        if (score.fontMaterial.HasProperty("_GlowColor"))
        {
            var glow = score.fontMaterial.GetColor("_GlowColor");
            logger.Msg($"glow color: {glow.r}, {glow.g}, {glow.b}");
            // some light blue about 75% on the gradient top to down
        }
        if (score.fontMaterial.HasProperty("_OutlineColor"))
        {
            var outline = score.fontMaterial.GetColor("_OutlineColor");
            logger.Msg($"outline color: {outline.r}, {outline.g}, {outline.b}");
            // 1 1 1, matches the screenshot
        }
    }
}
