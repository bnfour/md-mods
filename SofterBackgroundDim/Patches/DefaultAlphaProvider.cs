using HarmonyLib;
using MelonLoader;

using Il2Cpp;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppGameLogic;

namespace Bnfour.MuseDashMods.SofterBackgroundDim.Patches;

/// <summary>
/// Stores the default alpha for background brightness for later use within the mod
/// as it's set for the game scene.
/// </summary>
[HarmonyPatch(typeof(GameMusicScene), nameof(GameMusicScene.SetGameBrightnessColor))]
public class DefaultAlphaProvider
{
    internal static void Postfix()
    {
        Melon<SofterBackgroundDimMod>.Instance.DefaultAlpha
            = SingletonMonoBehaviour<GameSceneContainer>.instance.sprLightness.color.a;
    }
}
