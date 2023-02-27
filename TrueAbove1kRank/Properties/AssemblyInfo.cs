using System;
using System.Reflection;
using MelonLoader;

using Bnfour.MuseDashMods.TrueAbove1kRank;

// TODO github repo link?
[assembly: MelonInfo(typeof(TrueAbove1kRankMod), "True rank for 1k+", "0.1.0", "bnfour")]
[assembly: MelonGame("PeroPeroGames", "MuseDash")]
[assembly: MelonColor(ConsoleColor.Cyan)]
[assembly: MelonAuthorColor(ConsoleColor.Gray)]

[assembly: AssemblyDescription("Displays actual ranking instead of \"999+\" for ranks 999--2000")]
// TODO fill in
[assembly: AssemblyCopyright("© bnfour, open-source")]
