using MelonLoader;

namespace Bnfour.MuseDashMods.UITweaks;

public class UITweaksMod : MelonMod
{
    private MelonPreferences_Category _prefsCategory;
    private MelonPreferences_Entry<bool> _widerTitlesEnabled;
    private MelonPreferences_Entry<bool> _cupImageSyncEnabled;

    internal bool WiderAlbumTitlesEnabled => _widerTitlesEnabled.Value;
    internal bool AchievementIconsSyncEnabled => _cupImageSyncEnabled.Value;
    internal bool HpFeverFlowSyncEnabled => true;
    internal bool HpFeverFlowSyncUseAltMode => false;

    public override void OnInitializeMelon()
    {
        base.OnInitializeMelon();

        _prefsCategory = MelonPreferences.CreateCategory("Bnfour_UITweaks");
        _widerTitlesEnabled = _prefsCategory.CreateEntry("WiderAlbumTitles", true,
            "Wider album titles", "Enables wider album titles on song selection screen.");
        _cupImageSyncEnabled = _prefsCategory.CreateEntry("AchievementIconsSync", true,
            "Achievement icons sync", "Syncs the spinning cup images for song achievements.");

        if (!WiderAlbumTitlesEnabled && !AchievementIconsSyncEnabled && !HpFeverFlowSyncEnabled)
        {
            LoggerInstance.Warning("No features of the mod enabled, might as well uninstall it.");
        }
    }
}
