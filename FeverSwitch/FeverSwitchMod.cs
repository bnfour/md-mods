using MelonLoader;

using Bnfour.MuseDashMods.FeverSwitch.Utilities;

namespace Bnfour.MuseDashMods.FeverSwitch;

public class FeverSwitchMod : MelonMod
{
    internal SpriteNameProvider NameProvider = new();
    // TODO load pref
    internal SpriteProvider SpriteProvider = new(true);

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (sceneName == "UISystem_PC")
        {
            SpriteProvider.ResetCache();
        }
    }
}
