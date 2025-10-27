using MelonLoader;

using Bnfour.MuseDashMods.UITweaks.Utilities;

namespace Bnfour.MuseDashMods.UITweaks;

/// <summary>
/// Mod class that handles preferences, mostly to toggle different features.
/// </summary>
public class UITweaksMod : MelonMod
{
    private MelonPreferences_Category _prefsCategory;
    private MelonPreferences_Entry<bool> _widerTitlesEnabled;
    private MelonPreferences_Entry<bool> _cupImageSyncEnabled;
    private MelonPreferences_Entry<bool> _hpFeverSyncEnabled;
    private MelonPreferences_Entry<bool> _hpFeverSyncUseAltMode;
    private MelonPreferences_Entry<bool> _autoFeverNoticeEnabled;
    private MelonPreferences_Entry<bool> _optionButtonsFullCaps;
    private MelonPreferences_Entry<bool> _achievementsHeaderStyling;
    private MelonPreferences_Entry<bool> _charSelectAnimation;
    private MelonPreferences_Entry<bool> _tabularFonts;

    internal bool WiderAlbumTitlesEnabled => _widerTitlesEnabled.Value;
    internal bool AchievementIconsSyncEnabled => _cupImageSyncEnabled.Value;
    internal bool HpFeverFlowSyncEnabled => _hpFeverSyncEnabled.Value;
    internal bool HpFeverFlowSyncUseAltMode => _hpFeverSyncUseAltMode.Value;
    internal bool AutoFeverNoticeEnabled => _autoFeverNoticeEnabled.Value;
    internal bool FullCapsForOptionButtons => _optionButtonsFullCaps.Value;
    internal bool AchievementsHeaderClassicStyling => _achievementsHeaderStyling.Value;
    internal bool AnimateCharacterSelector => _charSelectAnimation.Value;
    internal bool ScoreboardTabularFonts => _tabularFonts.Value;

    internal FontChanger FontChanger => ScoreboardTabularFonts ? new() : null;

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
        _autoFeverNoticeEnabled = _prefsCategory.CreateEntry("AutoFeverText", true,
            "Auto fever text", "Changes text to \"AUTO\" on the fever bar if automatic fever is enabled.");
        _optionButtonsFullCaps = _prefsCategory.CreateEntry("OptionsFullCaps", true,
            "Uppercase for options", "Fixes some texts in options being not uppercase like the rest.");
        _achievementsHeaderStyling = _prefsCategory.CreateEntry("AchievementsHeaderStyling", true,
            "Classic achievements header styling", "Restores pre-5.6.0 Song info's achievements header styling.");
        _charSelectAnimation = _prefsCategory.CreateEntry("CharacterSelectAnimation", true,
            "Animate character selector appearance", "Adds an appearance animation for the character selector when song details screen is opened.");
        _tabularFonts = _prefsCategory.CreateEntry("TabularNumbersScoreboard", true,
            "Tabular numbers for scoreboard", "Makes score and accuracy numbers in the scoreboard monospace for easy comparing.");

        if (!WiderAlbumTitlesEnabled && !AchievementIconsSyncEnabled
            && !HpFeverFlowSyncEnabled && !AutoFeverNoticeEnabled
            && !FullCapsForOptionButtons && !AchievementsHeaderClassicStyling
            && !AnimateCharacterSelector && !ScoreboardTabularFonts)
        {
            LoggerInstance.Warning("No features of the mod enabled, might as well uninstall it.");
        }
    }
}
