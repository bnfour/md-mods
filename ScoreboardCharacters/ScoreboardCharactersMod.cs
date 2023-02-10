using MelonLoader;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

namespace Bnfour.MuseDashMods.ScoreboardCharacters
{
    public class ScoreboardCharactersMod : MelonMod
    {
        public readonly ButtonImageProvider ButtonImageProvider = new ButtonImageProvider();

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            if (sceneName == "UISystem_PC")
            {
                ButtonImageProvider.ResetCache();
            }
        }
    }
}
