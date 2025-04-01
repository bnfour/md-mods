using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Keeps the spinning award cup images on song screen synced regardless of difficulty
/// level switches -- by default, the animation is reset whenever the icon is shown.
/// </summary>
[HarmonyPatch(typeof(PnlPreparationLayAchv), nameof(PnlPreparationLayAchv.Refresh))]
public class PnlPreparationLayAchvRefreshPatch
{
    /// <summary>
    /// Called just before difficulty level switch. Passes the current animation
    /// state info to postfix, if necessary.
    /// </summary>
    /// <param name="__instance">Instance of the panel to perform sync on.</param>
    /// <param name="__state">Normalized animation time to sync animators to.
    /// If null, no sync is required because either there's no active icons or
    /// the feature is disabled altogether.</param>
    private static void Prefix(PnlPreparationLayAchv __instance, out float? __state)
    {
        // tell the postfix to do nothing if the feature is not enabled
        if (!Melon<UITweaksMod>.Instance.AchievementIconsSyncEnabled)
        {
            __state = null;
            return;
        }
        // store the first active animator's (if any) state to sync others to
        foreach (var animator in __instance.achvAnimators)
        {
            if (animator.enabled)
            {
                __state = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                return;
            }
        }
        __state = null;
    }

    /// <summary>
    /// Called after the difficulty level switch. Sets the previously saved
    /// animation state (if any) to the icons visible now.
    /// </summary>
    /// <param name="__instance">Instance of the panel to perform sync on.</param>
    /// <param name="__state">Normalized animation time to sync animators to.
    /// If null, no sync is required and this method does nothing.</param>
    private static void Postfix(PnlPreparationLayAchv __instance, float? __state)
    {
        if (__state.HasValue)
        {
            foreach (var animator in __instance.achvAnimators)
            {
                if (animator.enabled)
                {
                    animator.Play(0, 0, __state.Value);
                }
            }
        }
    }
}
