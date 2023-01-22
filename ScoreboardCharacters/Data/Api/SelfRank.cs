using System;
using Newtonsoft.Json;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data.Api
{
    public class SelfRank
    {
        [JsonProperty("detail")]
        public Detail Detail { get; set; }
        
        // also includes zero-based order
        // set to 999 for anything between 1000 and 2000
    }
}
