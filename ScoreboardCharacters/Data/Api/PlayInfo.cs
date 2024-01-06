using Newtonsoft.Json;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data.Api;

public class PlayInfo
{
    [JsonProperty("character_uid")]
    public string CharacterId { get; set; }
    [JsonProperty("elfin_uid")]
    public string ElfinId { get; set; }

    // also includes a lot of other fields,
    // some never shown anywhere in the vanilla UI
}
