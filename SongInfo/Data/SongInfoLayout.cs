namespace Bnfour.MuseDashMods.SongInfo.Data;

/// <summary>
/// Represents available UI layouts to show BPM and duration data.
/// </summary>
public enum SongInfoLayout
{
    /// <summary>
    /// New default, one line just above vanilla 5.6.0 character selector.
    /// </summary>
    OneLine,
    /// <summary>
    /// The old reliable, separate lines for BPM and duration. Requires moving the selector away
    /// (Scoreboard characters does that, among other things).
    /// </summary>
    TwoLines

    // more to come???4
}
