using Newtonsoft.Json;

namespace Bnfour.MuseDashMods.RankPreview.Data.Api;

/// <summary>
/// Wrapper class for a single scoreboard entry. We only care about play -> score,
/// other fields are not deserialized.
/// </summary>
public class ScoreboardEntry
{
    [JsonProperty("play")]
    public PlayInfo Info { get; set; }

    // there's also "user" section
}
