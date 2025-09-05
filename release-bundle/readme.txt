Please see https://github.com/bnfour/md-mods for full information, most importantly, the checksums.
===================================================================================================

Use these with MelonLoader 0.7.0
--------------------------------

The folders go into the game install folder
(the one where MuseDash.exe is, you'll see them if it's already modded)

Mods folder:
  AlbumScroll.dll : enables to scroll through current album using Shift keys
  RankPreview.dll : shows approximate rank (if in top 100) on song completion
  ScoreboardCharacters.dll : mod to show character/elfin info on the scoreboard
  SongInfo.dll : shows song's BPM and duration
  TrueAbove1kRank.dll : changes "999+" in the scoreboard to an actual rank
  UITweaks.dll : various small UI fixes

UserLibs folder, only required for ScoreboardCharacters:
  SkiaSharp.dll : https://www.nuget.org/packages/SkiaSharp
    net6.0 version
  libSkiaSharp.dll : https://www.nuget.org/packages/SkiaSharp.NativeAssets.Win32
    native win-x64 assets, also work on Proton as there's no native game version
