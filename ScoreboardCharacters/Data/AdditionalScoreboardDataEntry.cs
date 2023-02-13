using System;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data
{
    /// <summary>
    /// Represents data for a single custom button.
    /// </summary>
    public class AdditionalScoreboardDataEntry
    {
        public Character CharacterId { get; private set; }
        public Elfin ElfinId { get; private set; }

        public AdditionalScoreboardDataEntry(Api.PlayInfo detail)
        {
            CharacterId = (Character)int.Parse(detail.CharacterId);
            ElfinId = (Elfin)int.Parse(detail.ElfinId);
        }
    }
}
