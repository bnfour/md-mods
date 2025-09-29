using MelonLoader;

using Bnfour.MuseDashMods.RankPreview.Utilities;

namespace Bnfour.MuseDashMods.RankPreview;

public class RankPreviewMod : MelonMod

{
    /// <summary>
    /// Holds recently loaded scoreboards to estimate ranks against.
    /// </summary>
    internal readonly ScoreboardCache Cache = new();
}
