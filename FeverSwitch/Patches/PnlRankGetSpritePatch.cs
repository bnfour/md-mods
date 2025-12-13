using HarmonyLib;
using MelonLoader;
using UnityEngine;

using Il2CppAssets.Scripts.UI.Panels;

namespace Bnfour.MuseDashMods.FeverSwitch.Patches;

/// <summary>
/// Replaces the sprites used with the random toggle to match the current fever mode instead.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.GetSprite))]
public class PnlRankGetSpritePatch
{
    internal static void Postfix(string key, ref Sprite __result)
    {
        var mod = Melon<FeverSwitchMod>.Instance;

        // should in theory never happen, but who knows
        // also keeps null checks happy
        if (mod.SpriteProvider == null)
        {
            mod.LoggerInstance.Warning("SpriteProvider not initialized, toggle images will not be replaced");
            return;
        }

        if (key == mod.NameProvider.RandomOffSpriteName)
        {
            __result = mod.SpriteProvider.Off;
            return;
        }
        if (key == mod.NameProvider.RandomOnSpriteName)
        {
            __result = mod.SpriteProvider.On;
        }
    }
}
