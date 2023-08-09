using Newtonsoft.Json;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data.Api
{
    public class SelfRank
    {
        [JsonProperty("detail")]
        public PlayInfo Info { get; set; }
        
        // also includes zero-based order
    }
}
