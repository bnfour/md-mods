using System;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data
{
    public class AdditionalScoreboardDataEntry
    {
        public Character CharacterId { get; set; }
        public Elfin ElfinId { get; set; }

        // just in case, probably not necessary
        public AdditionalScoreboardDataEntry() { }

        public AdditionalScoreboardDataEntry(Api.PlayInfo detail)
        {
            CharacterId = (Character)int.Parse(detail.CharacterId);
            ElfinId = (Elfin)int.Parse(detail.ElfinId);
        }
    }
}
