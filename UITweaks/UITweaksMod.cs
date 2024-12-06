using MelonLoader;

namespace Bnfour.MuseDashMods.UITweaks;

public class UITweaksMod : MelonMod
{
    private MelonPreferences_Category _prefsCategory;
    private MelonPreferences_Entry<bool> _widerTitlesEnabled;
    private MelonPreferences_Entry<bool> _cupImageSyncEnabled;
    private MelonPreferences_Entry<bool> _hpFeverSyncEnabled;
    private MelonPreferences_Entry<bool> _hpFeverSyncUseAltMode;

    internal bool WiderAlbumTitlesEnabled => _widerTitlesEnabled.Value;
    internal bool AchievementIconsSyncEnabled => _cupImageSyncEnabled.Value;
    internal bool HpFeverFlowSyncEnabled => _hpFeverSyncEnabled.Value;
    internal bool HpFeverFlowSyncUseAltMode => _hpFeverSyncUseAltMode.Value;

    public override void OnInitializeMelon()
    {
        base.OnInitializeMelon();

        _prefsCategory = MelonPreferences.CreateCategory("Bnfour_UITweaks");
        _widerTitlesEnabled = _prefsCategory.CreateEntry("WiderAlbumTitles", true,
            "Wider album titles", "Enables wider album titles on song selection screen.");
        _cupImageSyncEnabled = _prefsCategory.CreateEntry("AchievementIconsSync", true,
            "Achievement icons sync", "Syncs the spinning cup images for song achievements.");
        _hpFeverSyncEnabled = _prefsCategory.CreateEntry("SyncHpFeverAnim", true,
            "HP and Fever bar animation sync", "Syncs the bubble animation for HP and Fever bars.");
        _hpFeverSyncUseAltMode = _prefsCategory.CreateEntry("SyncHpFeverAnimAlt", false,
            "Alternate HP-Fever sync", "Syncs HP bar to Fever bar.");

        if (!WiderAlbumTitlesEnabled && !AchievementIconsSyncEnabled && !HpFeverFlowSyncEnabled)
        {
            LoggerInstance.Warning("No features of the mod enabled, might as well uninstall it.");
        }
    }
}
