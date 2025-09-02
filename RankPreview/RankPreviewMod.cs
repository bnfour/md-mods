using System.Collections.Generic;
using MelonLoader;

namespace Bnfour.MuseDashMods.RankPreview;

public class RankPreviewMod : MelonMod

{
    // TODO something more typed and with limited capacity

    // uid+diff -> scores array
    internal readonly Dictionary<string, int[]> Cache = new();
}
