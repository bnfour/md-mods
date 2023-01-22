using System;

using MelonLoader;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters
{
    // entry point
    // and probably the place to store shared data because we can easily get a reference to it from patchers
    public class ScoreboardCharactersMod : MelonMod
    {
        public readonly AdditionalScoreboardData ScoreboardData = new AdditionalScoreboardData();

        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            LoggerInstance.Msg("hello world");
        }
    }
}
