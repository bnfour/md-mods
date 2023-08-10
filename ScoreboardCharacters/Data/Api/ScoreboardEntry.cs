using Newtonsoft.Json;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data.Api
{
    public class ScoreboardEntry
    {
        [JsonProperty("play")]
        public PlayInfo Info { get; set; }

        // also includes user details,
        // not used there
    }
}
