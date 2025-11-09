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
        var nameProvider = Melon<FeverSwitchMod>.Instance.NamesProvider;
        // TODO actually replace the sprites
        if (key == nameProvider.RandomOffSpriteName)
        {
            Melon<FeverSwitchMod>.Logger.Msg($"wanna switch sprite for off sprite {key}");
        }
        if (key == nameProvider.RandomOnSpriteName)
        {
            Melon<FeverSwitchMod>.Logger.Msg($"wanna switch sprite for on sprite {key}");
        }
    }
}
