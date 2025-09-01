using Bnfour.MuseDashMods.SongInfo.Patches;

namespace Bnfour.MuseDashMods.SongInfo.Utilities.UI;

/// <summary>
/// Generator for the default one line layout. Only sets up the component name --
/// all other logic is covered by the base class.
/// </summary>
internal sealed class TopRightGeneratorOneLine : TopRightGeneratorBase, IComponentGenerator
{
    protected override string FirstLineComponentName => Constants.TopRight.OneLine;
}
