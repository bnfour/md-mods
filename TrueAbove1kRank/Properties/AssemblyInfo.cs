using System;
using System.Reflection;
using MelonLoader;

using Bnfour.MuseDashMods.TrueAbove1kRank;

[assembly: MelonInfo(typeof(TrueAbove1kRankMod), "True rank for 999+", "1.0.0", "bnfour", "https://github.com/bnfour/md-mods")]
[assembly: MelonGame("PeroPeroGames", "MuseDash")]
[assembly: MelonColor(ConsoleColor.Cyan)]
[assembly: MelonAuthorColor(ConsoleColor.Gray)]

[assembly: AssemblyDescription("Displays actual ranking instead of \"999+\" for ranks 999--2000")]
[assembly: AssemblyCopyright("bnfour 2023; open-source")]
