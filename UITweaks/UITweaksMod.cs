using MelonLoader;

namespace Bnfour.MuseDashMods.UITweaks;

public class UITweaksMod : MelonMod
{
    private MelonPreferences_Category _prefsCategory;
    private MelonPreferences_Entry<bool> _widerTitlesEnabled;

    internal bool WiderAlbumTitlesEnabled => _widerTitlesEnabled.Value;
    internal bool AchievementIconsSyncEnabled => true;
    internal bool HpFeverFlowSyncEnabled => true;
    internal bool HpFeverFlowSyncUseAltMode => false;

    public override void OnInitializeMelon()
    {
        base.OnInitializeMelon();

        _prefsCategory = MelonPreferences.CreateCategory("Bnfour_UITweaks");
        _widerTitlesEnabled = _prefsCategory.CreateEntry("WiderAlbumTitles", true,
            "Wider album titles", "Enables wider album titles on song selection screen.");

        if (!WiderAlbumTitlesEnabled)
        {
            LoggerInstance.Warning("No features of the mod enabled, might as well uninstall it.");
        }
    }
}
