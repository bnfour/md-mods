using MelonLoader;

namespace Bnfour.MuseDashMods.UITweaks;

public class UITweaksMod : MelonMod
{
    private MelonPreferences_Category _prefsCategory;

    internal bool WiderAlbumTitlesEnabled => true;

    public override void OnInitializeMelon()
    {
        base.OnInitializeMelon();

        _prefsCategory = MelonPreferences.CreateCategory("Bnfour_UITweaks");

        if (true)
        {
            LoggerInstance.Warning("No features of the mod enabled, might as well uninstall it.");
        }
    }
}
