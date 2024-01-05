using System.Collections.Generic;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data;

/// <summary>
/// Represents data for all the extra buttons on a level scoreboard.
/// </summary>
public class AdditionalScoreboardData
{
    /// <summary>
    /// Data for the current player, displayed at the bottom of the scoreboard.
    /// Can be null if the player is not in top 2000 for the song/diff.
    /// </summary>
    public AdditionalScoreboardDataEntry Self { get; set; }

    /// <summary>
    /// Data for the scoreboard. May include an entry duplicating <see cref="Self"/>,
    /// if the player is good enough.
    /// </summary>
    public List<AdditionalScoreboardDataEntry> Scoreboard { get; } = new List<AdditionalScoreboardDataEntry>(99);
}
