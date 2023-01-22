using System;
using System.Collections.Generic;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data
{
    public class AdditionalScoreboardData
    {
        /// <summary>Data for the current player, displayed at the bottom of the scoreboard.</summary>
        public AdditionalScoreboardDataEntry SelfData { get; set; }

        /// <summary>Data for the scoreboard. May include an entry duplicationg <see cref="SelfData"/>,
        /// if the player is good enough.</summary>
        public List<AdditionalScoreboardData> ScoreboardData { get; set; }

        public void Clear()
        {
            SelfData = null;
            ScoreboardData = new List<AdditionalScoreboardData>(99);
        }
    }
}
