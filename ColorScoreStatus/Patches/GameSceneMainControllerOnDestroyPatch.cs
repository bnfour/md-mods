using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Patches;

/// <summary>
/// Resets the combo state whenever the game screen is destroyed.
/// </summary>
[HarmonyPatch(typeof(GameSceneMainController), nameof(GameSceneMainController.OnDestroy))]
public class GameSceneMainControllerOnDestroyPatch
{
    internal static void Postfix()
    {
        Melon<ColorScoreStatusMod>.Instance.ResetState();
    }
}
