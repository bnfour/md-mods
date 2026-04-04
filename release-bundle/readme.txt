Please see https://github.com/bnfour/md-mods for full information, most importantly, the checksums.
===================================================================================================

Use these with MelonLoader 0.7.2
--------------------------------

The folders go into the game install folder
(the one where MuseDash.exe is, you'll see them if it's already modded)

Mods folder:
  AlbumScroll.dll : enables to scroll through current album using Shift keys
  FeverSwitch.dll : turns random character/elfin switch into auto/manual fever switch
  RankPreview.dll : shows approximate rank (if in top 100) on song completion
  ScoreboardCharacters.dll : mod to show character/elfin info on the scoreboard
  SongInfo.dll : shows song's BPM and duration
  TrueAbove1kRank.dll : changes "999+" in the scoreboard to an actual rank
  UITweaks.dll : various small UI fixes

UserLibs folder, only required for Scoreboard characters and Song info:
  SkiaSharp.dll : https://www.nuget.org/packages/SkiaSharp
    required by Scoreboard characters, net6 version

  libSkiaSharp.dll : https://www.nuget.org/packages/SkiaSharp.NativeAssets.Win32
    required by Scoreboard characters
    native win-x64 assets, also work on Proton as there's no native game version

  K4os.Compression.LZ4.dll : https://www.nuget.org/packages/K4os.Compression.LZ4
    technically optional for Song info, _highly_ recommended for best experience
