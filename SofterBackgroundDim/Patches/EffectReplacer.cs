using System;
using HarmonyLib;
using MelonLoader;

using Il2Cpp;
using Il2CppAssets.Scripts.GameCore.GameObjectLogics.GameObjectManager;
using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppAssets.Scripts.PeroTools.Commons;

namespace Bnfour.MuseDashMods.SofterBackgroundDim.Patches;

/// <summary>
/// Actually disables the black background effect and replaces it
/// with a custom implementation that never completely disables the background.
/// </summary>
[HarmonyPatch(typeof(GameSceneHideController), nameof(GameSceneHideController.OnControllerStart))]
public class EffectReplacer
{
    internal static void Postfix()
    {
        // disable the original effect (only needed when turning the effect on)
        AttackEffectManager.instance.sprGameSceneHide.enabled = false;

        // set the required brightness (alpha)

        var alpha = Singleton<BattleProperty>.instance.isHideGameScene
            ? Melon<SofterBackgroundDimMod>.Instance.DimmedAlpha
            : Melon<SofterBackgroundDimMod>.Instance.DefaultAlpha ?? throw new ApplicationException("Default alpha value not available.");
        // TODO while this (^) should never happen, consider failing more gracefully

        SingletonMonoBehaviour<GameSceneContainer>.instance.sprLightness.color
            = new(0f, 0f, 0f, alpha);
    }
}
