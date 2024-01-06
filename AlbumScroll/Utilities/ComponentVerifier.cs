using Il2CppAssets.Scripts.PeroTools.Nice.Components;

namespace Bnfour.MuseDashMods.AlbumScroll.Utilities;

/// <summary>
/// Used to check that the FancyScrollView instance we're about to modify
/// is active and actually used for song scrolling.
/// The same FancyScrollView class is also used for elfin and character selection,
/// and these should not be affected.
/// </summary>
public static class ComponentVerifier
{
    public static bool IsActiveSongScrollView(FancyScrollView instance)
    {
        return instance.isActiveAndEnabled
            && instance.transform?.parent?.name == "MusicRoot";
    }
}
