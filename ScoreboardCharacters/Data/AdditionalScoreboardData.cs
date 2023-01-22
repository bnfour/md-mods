using System;
using System.Collections.Generic;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data
{
    public class AdditionalScoreboardData
    {
        /// <summary>Data for the current player, displayed at the bottom of the scoreboard.</summary>
        public AdditionalScoreboardDataEntry Self { get; set; }

        /// <summary>Data for the scoreboard. May include an entry duplicationg <see cref="Self"/>,
        /// if the player is good enough.</summary>
        public List<AdditionalScoreboardDataEntry> Scoreboard { get; set; }

        public void Clear()
        {
            Self = null;
            Scoreboard = new List<AdditionalScoreboardDataEntry>(99);
        }
    }
}
