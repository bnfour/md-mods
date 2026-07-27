using System.Reflection;
using System.Runtime.CompilerServices;
using MelonLoader;

using Bnfour.MuseDashMods.SofterBackgroundDim;

[assembly: MelonInfo(typeof(SofterBackgroundDimMod), "Softer background dim", "0.0.1", "bnfour", "https://github.com/bnfour/md-mods")]
[assembly: MelonGame("PeroPeroGames", "MuseDash")]
[assembly: MelonColor(255, 202, 80, 16)]
[assembly: MelonAuthorColor(255, 128, 128, 128)]

[assembly: AssemblyDescription("Replaces completely black background effect with a darker background")]
[assembly: AssemblyCopyright("bnfour 2026; open-source")]

[assembly: InternalsVisibleTo("Tests")]
