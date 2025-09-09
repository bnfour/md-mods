using Il2Cpp;

namespace Bnfour.MuseDashMods.SongInfo.Utilities.UI.Setting;

internal interface IDataSetter
{
    /// <summary>
    /// Sets the data to the UI created earlier, depending on configured layout.
    /// </summary>
    /// <param name="panel">Panel instance to look for the custom UI in.</param>
    /// <param name="bpm">BPM to set -- taken from game's database, preformatted.</param>
    /// <param name="duration">Duration to set, preformatted.</param>
    void Set(PnlPreparation panel, string bpm, string duration);
}
