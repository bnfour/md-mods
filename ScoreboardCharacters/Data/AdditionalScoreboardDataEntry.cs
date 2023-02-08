using System;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data
{
    public class AdditionalScoreboardDataEntry
    {
        public string CharacterId { get; set; }
        public string ElfinId { get; set; }

        // just in case, probably not necessary
        public AdditionalScoreboardDataEntry() { }

        public AdditionalScoreboardDataEntry(Api.PlayInfo detail)
        {
            CharacterId = detail.CharacterId;
            ElfinId = detail.ElfinId;
        }

        public override string ToString()
        {
            // human-readable proved to be too long for a name label copy we have now
            return $"{CharacterId}/{ElfinId}";
        }
    }
}
