using Newtonsoft.Json;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data.Api;

public class PlayInfo
{
    [JsonProperty("character_uid")]
    public string CharacterId { get; set; }

    // only set if character_uid is 36, represents character whose skill was used
    [JsonProperty("character_skill_uid")]
    public string KaitouActualId { get; set; }

    [JsonProperty("elfin_uid")]
    public string ElfinId { get; set; }

    // also includes a lot of other fields,
    // some never shown anywhere in the vanilla UI
}
