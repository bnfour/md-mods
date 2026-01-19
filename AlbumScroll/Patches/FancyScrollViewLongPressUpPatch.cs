using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

using Il2CppAssets.Scripts.PeroTools.Nice.Components;

using Bnfour.MuseDashMods.AlbumScroll.Components;
using Bnfour.MuseDashMods.AlbumScroll.Data;
using Bnfour.MuseDashMods.AlbumScroll.Utilities;

namespace Bnfour.MuseDashMods.AlbumScroll.Patches;

[HarmonyPatch]
public class FancyScrollViewLongPressUpPatch
{
    internal static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(FancyScrollView), nameof(FancyScrollView.LongPressPreviousUp));
        yield return AccessTools.Method(typeof(FancyScrollView), nameof(FancyScrollView.LongPressNextUp));
    }

    internal static void Postfix(FancyScrollView __instance, MethodBase __originalMethod)
    {
        if (__instance.gameObject.GetComponent<ShiftStateTracker>() is ShiftStateTracker stateTracker
            && stateTracker.ShiftDown)
        {
            var dir = __originalMethod.Name switch
            {
                nameof(FancyScrollView.LongPressNextUp) => Direction.Forward,
                nameof(FancyScrollView.LongPressPreviousUp) => Direction.Backward,
                _ => throw new System.ApplicationException("Unknown FSV directional press method.")
            };

            AlbumScroller.ScrollToDifferentAlbum(__instance, dir);
        }
    }
}
