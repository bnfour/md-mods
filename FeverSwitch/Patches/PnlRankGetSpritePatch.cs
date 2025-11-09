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

        if (key == mod.NameProvider.RandomOffSpriteName)
        {
            __result = mod.SpriteProvider.Off;
        }
        if (key == mod.NameProvider.RandomOnSpriteName)
        {
            __result = mod.SpriteProvider.On;
        }
    }
}
