using HarmonyLib;
using Il2Cpp;
using MelonLoader;

using Bnfour.MuseDashMods.ColorScoreStatus.Utilities.ColorSetting;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Patches;

/// <summary>
/// Initializes the score coloring code based on the UI mode the game just set.
/// </summary>
[HarmonyPatch(typeof(ChangeScoreValue), nameof(ChangeScoreValue.InitUi))]
public class ChangeScoreValueInitUiPatch
{
    internal static void Postfix(ChangeScoreValue __instance)
    {
        IScoreColorer colorer = __instance.m_Index switch
        {
            0 => new BasicScoreColorer(__instance.text.gameObject),
            1 => new GCScoreColorer(__instance.gcText.gameObject),
            2 => new BasicScoreColorer(__instance.djmaxText.gameObject),
            3 => new BasicScoreColorer(__instance.arkNightText.gameObject),
            _ => null
        };

        if (colorer == null)
        {
            Melon<ColorScoreStatusMod>.Logger.Warning($"Unknown UI mode {__instance.m_Index}, not doing anything.");
        }

        Melon<ColorScoreStatusMod>.Instance.scoreColorer = colorer;
        // color to the default state
        colorer?.SetStateTo(Data.ComboStatus.AllPerfect);
    }
}
