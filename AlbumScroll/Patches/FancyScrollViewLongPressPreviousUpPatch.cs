using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.PeroTools.Nice.Components;

using Bnfour.MuseDashMods.AlbumScroll.Data;
using Bnfour.MuseDashMods.AlbumScroll.Utilities;

namespace Bnfour.MuseDashMods.AlbumScroll.Patches;

[HarmonyPatch(typeof(FancyScrollView), nameof(FancyScrollView.LongPressPreviousUp))]
public class FancyScrollViewLongPressPreviousUpPatch
{
    private static void Postfix(FancyScrollView __instance)
    {
        if (Melon<AlbumScrollMod>.Instance.ShiftDown
            && ComponentVerifier.IsActiveSongScrollView(__instance))
        {
            AlbumScroller.ScrollToDifferentAlbum(__instance, Direction.Backward);
        }
    }
}
