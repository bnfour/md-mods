using Il2Cpp;

namespace Bnfour.MuseDashMods.SongInfo.Utilities.UI;

internal interface IComponentGenerator
{
    /// <summary>
    /// Creates a mod UI based on selected layout.
    /// </summary>
    /// <param name="instance">Panel instance to work on.</param>
    void CreateModUI(PnlPreparation instance);
}
