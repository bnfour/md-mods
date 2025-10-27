using HarmonyLib;
using MelonLoader;
using UnityEngine;

using Il2Cpp;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Changes the font for numbers in the scoreboard proper to a custom one
/// with numbers of same width for easy comparing.
/// </summary>
[HarmonyPatch(typeof(FastPoolManager), nameof(FastPoolManager.CreatePool), new[] { typeof(GameObject), typeof(bool), typeof(int), typeof(int), typeof(Transform) })]
public class FastPoolManagerCreatePoolPatch
{
    internal static void Prefix(GameObject prefab)
    {
        var fontChanger = Melon<UITweaksMod>.Instance.FontChanger;
        // fontChanger being null implies than the feature is disabled
        if (fontChanger != null && prefab.name == "RankCell_4-3")
        {
            fontChanger.ChangeNumericFonts(prefab);
        }
    }
}
