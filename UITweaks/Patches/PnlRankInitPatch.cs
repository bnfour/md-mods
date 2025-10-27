using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.UI.Panels;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Changes the font for numbers in the self rank line to a custom one with numbers
/// of the same width for easy comparing.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.Init))]
public class PnlRankInitPatch
{
    internal static void Postfix(PnlRank __instance)
    {
        var fontChanger = Melon<UITweaksMod>.Instance.FontChanger;
        // fontChanger being null implies than the feature is disabled
        if (fontChanger != null)
        {
            fontChanger.ChangeNumericFonts(__instance.server);
        }
    }
}
