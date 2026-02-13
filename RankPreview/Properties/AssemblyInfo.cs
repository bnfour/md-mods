using System.Reflection;
using System.Runtime.CompilerServices;
using MelonLoader;

using Bnfour.MuseDashMods.RankPreview;

[assembly: MelonInfo(typeof(RankPreviewMod), "Rank preview", "1.0.2", "bnfour", "https://github.com/bnfour/md-mods")]
[assembly: MelonGame("PeroPeroGames", "MuseDash")]
[assembly: MelonColor(255, 202, 80, 16)]
[assembly: MelonAuthorColor(255, 128, 128, 128)]

[assembly: AssemblyDescription("Shows estimated scoreboard rank (if in top 100) on stats screen")]
[assembly: AssemblyCopyright("bnfour 2025-2026; open-source")]

[assembly: InternalsVisibleTo("Tests")]
