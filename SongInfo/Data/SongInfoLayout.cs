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
    TwoLines,
    /// <summary>
    /// Alternative layout that uses free space in the best record panel below the vanilla data.
    /// This is actually the very first layout I have considered for implementation before moving on to the one now known
    /// as two lines layout. Mostly because the level design label is exposed in the code, so it's way easier to find and tinker with.
    /// </summary>
    BestRecord
}
