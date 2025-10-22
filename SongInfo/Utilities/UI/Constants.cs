namespace Bnfour.MuseDashMods.SongInfo.UI;

/// <summary>
/// Holds constants shared between other classes in the namespace.
/// </summary>
internal class Constants
{
    internal class TopRight
    {
        // entire top right component
        internal const string Component = "BnSongInfoClassicTopRight";

        // one line:

        // original scrolling text
        internal const string OneLine = "BnSongInfoOneLineMask";


        // two lines:

        // original scrolling text
        internal const string TwoLinesBpm = "BnSongInfoTwoLinesBpmMask";
        // clone of the original scrolling text placed below
        internal const string TwoLinesDuration = "BnSongInfoTwoLinesDurationMask";
    }

    internal class BestRecordPanel
    {
        // relative to the animated component
        internal const string ValuePath = "TxtValue";

        internal const string BpmComponent = "BnSongInfoBestRecordBpm";
        internal const string BpmTxt = "TxtBpm";
        internal const string BpmPath = $"Record/{BpmComponent}/{BpmTxt}";

        internal const string DurationComponent = "BnSongInfoBestRecordDuration";
        internal const string DurationTxt = "TxtDuration";
        internal const string DurationPath = $"Record/{DurationComponent}/{DurationTxt}";
    }
}
