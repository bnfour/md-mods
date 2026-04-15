using MelonLoader;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;
using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities.ButtonImages;

namespace Bnfour.MuseDashMods.ScoreboardCharacters;

public class ScoreboardCharactersMod : MelonMod
{
    public readonly ButtonImageProvider ButtonImageProvider = new();
    public readonly CharacterSwitcher CharacterSwitcher = new();

    internal bool OncePerSceneUpdatesApplied = false;

    /// <summary>
    /// Indicates that the custom UI should be updated with animated transition
    /// when user actually clicks a custom button. All other cases (init, difficulty
    /// change) just replace the sprite outright.
    /// </summary>
    internal bool AnimateNextConfigUpdate = false;

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasUnloaded(buildIndex, sceneName);

        if (sceneName == "UISystem_PC")
        {
            ButtonImageProvider.ResetCache();
            CharacterSwitcher.ResetCache();
            OncePerSceneUpdatesApplied = false;
        }
    }
}
