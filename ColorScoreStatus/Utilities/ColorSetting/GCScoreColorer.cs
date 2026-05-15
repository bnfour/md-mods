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
        Melon<ColorScoreStatusMod>.Logger.Msg($"just pretend the status was visibly set to {status} LULE");
    }
}
