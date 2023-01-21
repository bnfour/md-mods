using System;
using MelonLoader;

namespace Bnfour.MuseDashMods.ScoreboardCharacters
{
    // entry point
    // and probably the place to store shared data because we can easily get a reference to it from patchers
    public class ScoreboardCharactersMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            LoggerInstance.Msg("hello world");
        }
    }
}
