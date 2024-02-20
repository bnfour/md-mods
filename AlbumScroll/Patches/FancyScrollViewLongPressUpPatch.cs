using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.PeroTools.Nice.Components;

using Bnfour.MuseDashMods.AlbumScroll.Data;
using Bnfour.MuseDashMods.AlbumScroll.Utilities;

namespace Bnfour.MuseDashMods.AlbumScroll.Patches;

[HarmonyPatch]
public class FancyScrollViewLongPressUpPatch
{
    // see https://harmony.pardeike.net/articles/patching-auxiliary.html#targetmethods
    // and https://harmony.pardeike.net/articles/patching-injections.html#__originalmethod
    // for details on patching multiple similar methods with a single patch
    // it's pretty cool
    
    private static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(FancyScrollView), nameof(FancyScrollView.LongPressPreviousUp));
        yield return AccessTools.Method(typeof(FancyScrollView), nameof(FancyScrollView.LongPressNextUp));
    }

    [HarmonyPostfix]
    private static void Postfix(FancyScrollView __instance, MethodBase __originalMethod)
    {
        if (Melon<AlbumScrollMod>.Instance.ShiftDown
            && ComponentVerifier.IsActiveSongScrollView(__instance))
        {
            var dir = __originalMethod.Name.Contains("Next")
                ? Direction.Forward
                : Direction.Backward;

            AlbumScroller.ScrollToDifferentAlbum(__instance, dir);
        }
    }
}
