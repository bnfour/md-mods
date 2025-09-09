namespace Bnfour.MuseDashMods.RankPreview.Data.Api;

/// <summary>
/// Subset of the "play" section for a scoreboard entry.
/// We only care about the score value.
/// </summary>
public struct PlayInfo
{
    public int Score { get; set; }

    // a lot of fields omitted
}
