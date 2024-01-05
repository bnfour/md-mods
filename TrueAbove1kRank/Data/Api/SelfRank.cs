using Newtonsoft.Json;

namespace Bnfour.MuseDashMods.TrueAbove1kRank.Data.Api;

public class SelfRank
{
    [JsonProperty("order")]
    public int RawOrder { get; set; }

    // the API order is zero-based, but the scoreboard is not
    public int DisplayRank => RawOrder + 1;
}
