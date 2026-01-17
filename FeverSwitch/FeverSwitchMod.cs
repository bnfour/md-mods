using MelonLoader;

using Bnfour.MuseDashMods.FeverSwitch.Utilities;

namespace Bnfour.MuseDashMods.FeverSwitch;

public class FeverSwitchMod : MelonMod
{
    private MelonPreferences_Category? _prefsCategory;
    private MelonPreferences_Entry<bool>? _isAutoDefault;

    internal bool IsAutoDefault => _isAutoDefault?.Value ?? throw new System.ApplicationException("Preferences not loaded (yet), unable to load value for IsAutoDefault");

    internal readonly SpriteNameProvider NameProvider = new();
    internal SpriteProvider? SpriteProvider;

    public override void OnInitializeMelon()
    {
        _prefsCategory = MelonPreferences.CreateCategory("Bnfour_FeverSwitch");
        _isAutoDefault = _prefsCategory.CreateEntry("AutoIsDefault", true,
            "Auto fever is default", "Treat automatic fever mode as default, shown as toggle turned off.");

        SpriteProvider = new(_isAutoDefault.Value);
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (sceneName == "UISystem_PC")
        {
            SpriteProvider?.ResetCache();
        }
    }
}
