using MelonLoader;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;
using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities.ButtonImages;

namespace Bnfour.MuseDashMods.ScoreboardCharacters;

public class ScoreboardCharactersMod : MelonMod
{
    public readonly ButtonImageProvider ButtonImageProvider = new();
    public readonly CharacterSwitcher CharacterSwitcher = new();

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasUnloaded(buildIndex, sceneName);

        if (sceneName == "UISystem_PC")
        {
            ButtonImageProvider.ResetCache();
            CharacterSwitcher.ResetCache();
        }
    }
}
