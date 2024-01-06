Unofficial quality of life modifications for the PC version of the hit video game Muse Dash using MelonLoader.

# Disclaimers
- These mods are unofficial and are not associated with, related to, and/or endorsed by peropero, hasuhasu, and/or XD.
- USE AT YOUR OWN RISK. NO WARRANTIES.
- Please read [FAQ](#frequently-asked-questions) and have a look at [known issues](https://github.com/bnfour/md-mods/issues).

# Warning: runtime version switch!
Starting from version 8, the mods are built for .NET 6 and MelonLoader 0.6.1, as opposed to .NET 4.7.2 and MelonLoader 0.5.7 for earlier versions. To continue using latest versions of my mods, you should update the loader.

The main reason for this is that MelonLoader 0.5.7 just refused to launch on Proton after 3.12.1 patch of the game for whatever reason, so I had to move to newer version that works for me.

The source for old versions (1.x.x, v7 and earlier) of the mods is available in [`legacy-framework`](https://github.com/bnfour/md-mods/tree/legacy-framework) branch. As long as the old modloader itself continues to work for you, these should too. I'll probably keep updating the legacy Scoreboard characters with new characters and elfins for some time.

# Mod list
Currently, this repo contains three mods: two are scoreboard related, and another one enhances song select screen. They can be used together.

- [Scoreboard characters](#scoreboard-characters) — shows character/elfin info on the scoreboard
- [True rank](#true-rank) — changes "999+" in the scoreboard to an actual rank
- [Album scroll](#album-scroll) — enables to scroll through current album using Shift keys

## Scoreboard characters
Mod file: `ScoreboardCharacters.dll`, also requires `UserLibs` DLLs

This mod adds buttons to show charater and elfin used to obtain the score to the in-game scoreboard:

![image simulated for variety, real scoreboards are pretty boring most of the time](readme-images/scoreboard-characters.png)

These buttons can be clicked, and will set your current character and elfin to these on the button. It will even scroll the selection screens for you.

### Image override (advanced)
If you don't like the provided default images on the buttons or the way they are scaled on your screen resolution, they can be changed by providing an override spritesheet.

Please note that the override is designed to **not** apply any scaling to the images, and the sprites will be placed to the buttons as is.

#### Image preparation
A good way to start with an override is to use the default spritesheet for your resolution as a template. The defaults are located in [`ScoreboardCharacters/Resources`](ScoreboardCharacters/Resources) folder. There's also a bigger source and/or reference image in [`ScoreboardCharacters/etc`](ScoreboardCharacters/etc).

If there is no default image for your desired resolution, the source/reference can be scaled to form a template. The rest of this section describes the requirements the override image must meet; see ["Image override"](#image-override) for the way to enable the override image.

##### Sprite size
First, pick a size for individual sprites. It's best to use the size that matches the button size for your screen resolution. As a baseline, 〇×1080 resolution uses 40 px sprites. Scale this for your resolution.

For example, 2560×1440 screen size will work best with  
40 × 1440 / 1080 = 53 px  
sprite size.

##### Spritesheet resolution
The spritesheet must have 8 sprites (5 characters, 3 elfins) per row; so its width should be 8 times the width of a single sprite. The height must also be divisible by the sprite size, as the sprites are square.  
There should be enough rows to cover all existing characters and elfins; for now, an 8×8 grid will suffice.

Continuing the example, the size of custom spritesheet for 2560×1440 should be:
- width: 8 × 53 = 424 px
- height: 8 × 53 = 424 px

##### Sprites placement
Use the [reference image](ScoreboardCharacters/etc/sprites.png) to place the sprites on the spritesheet. The sprites with the numbers are placeholders for possible future updates, it's not mandatory to fill them in an override.

#### Image override
To override the default image, place your custom spritesheet as `scoreboard_characters_override.png` to `MuseDash_Data` directory of the game install. The override will be silently ignored if its dimensions differ from expected values. If you don't see your custom images in the game, please double-check the file name, location, and resolution.

## True rank
Mod file: `TrueAbove1kRank.dll`

This small mod changes mysterious "999+" text in your own scoreboard entry to your actual rank if it lies within 1000–2000 range:

![simulated image, no (you)s were harmed during production](readme-images/true-rank-showcase.png)

Please note that the game servers only track top 2000 entries. If you score less, it won't be tracked and there's nothing this mod can do.

## Album scroll
Mod file: `AlbumScroll.dll`

This mod adds an option to quickly scroll to the next album in the song selection menu. Just hold Shift key while scrolling (A/D, ←/→, mouse wheel, on-screen buttons — works with all of these). This will scroll to the closest song from a different album.

Here's a little demo of switching entire albums by single Shift+Arrow key taps:

https://github.com/bnfour/md-mods/assets/853426/92bb0375-95cb-40d3-81a8-8972ba9207af

Please note that this mod does not play well with "hold to scroll" feature. It might skip an album if a direction key is held long enough to trigger continuous scrolling mode. It's best to use single taps to scroll albums.

## Experimental mod
This is not a mod intended for using. Rather, it's a developmental test bed for me to test random stuff without changing existing proper mods. For instance, Album scroll mod was first implemented (in a very scuffed way; no, I won't show the code ⇀‸↼‶) in this project and then moved to its own permanent project.

The project contains the bare minimum for a mod that is successfully loaded; it does nothing except posting a single message in the log.

# Installation
These are [MelonLoader](https://melonwiki.xyz/) mods. In order to run these, you need to have it installed. Currently, 0.6.1 Open-Beta of MelonLoader is supported.  
Once you have MelonLoader installed, drop the DLLs of desired mods into the `Mods` folder. Remove to uninstall.  
Scoreboard characters mod also requires additional libraries to be placed in `UserLibs` folder.

Rather than downloading these, I suggest (reviewing the source and) building them yourself — this way you'll be sure the mods behave as described. See ["Building from source"](#building-from-source).  
Otherwise, please verify the downloads.

## Verification
Every published release is accompanied with SHA256 hashes of every DLL. MelonLoader does print these in console when loading mods, but I suggest to verify the hashes before installation.

# Frequently Asked Questions
(or, more accurately, "I thought you may want to know this")

### Is this cheating?
_tl;dr: no_

The scoreboard mods show the information the game already receives directly from its API, it's just not shown anywhere by default. You can already get this info, for instance, from [musedash.moe](https://musedash.moe/) scoreboard. In fact, this repo is born from my frustration of having to mirror my track selection in-game to the website on another display. The Album scroll mod only affects the song selection menu.  
The mods provide no advantage for the actual gameplay, only some convinence in song selection stage. You still have to git gud to earn high scores; you just know what character to pick and where exactly on the scoreboard you are.

Unless you count _any_ changes to the game for _any_ purpose as cheating, this is not cheating.

### Will I get banned for using these?
_tl;dr: probably not, but NO WARRANTIES; USE AT YOUR OWN RISK_

As I stated in previous question, I don't believe this is cheating. I've been using these continiously, and my account is still there. But there's a reason for the all-caps section of the license about having no warranties: the devs might think otherwise or break the compatibility (un)intentionally.

Remember that you're using the mods **at your own risk**. I have warned you many times in this readme.

### I have other mods. What about compatibility with them?
_tl;dr: ¯\\\_(ツ)\_/¯_

The mods are pretty much self-contained, so I think ("think" being the operative word here) they will work with other mods, unless these other mods change the vanilla code too much.

### My game is broken because of you and your mods, how can I fix this and blame you?
_tl;dr: uninstall, and remember: NO WARRANTIES_

If you just want to play the game, removing the mods (and maybe the modloader itself) is always an option.
* Please make sure you're using supported (**0.6.1**) version of MelonLoader.
* Try to remove mods not from this repo.
* Try to remove mods and/or modloader and check whether the vanilla game is broken too.

If none of these helps, feel free to submit an issue, unless it's already have been reported.

# Building from source
This repo is a run-of-the-mill .NET solution targeting .NET 6.

The only gotcha is that some libraries required to build it are not included because of file size (and licensing) issues. Your installation of MelonLoader will generate them for you.

Copy everything from `MelonLoader/Managed`, `MelonLoader/Il2CppAssemblies`, and `MelonLoader/net6` folders from the game install to the `references` folder of this repo. All the DLLs should be directly in the `references` folder, no subfolders.


This should cover the local references for all the projects. (Actually, **most** of the DLLs are not necessary to build the solution, I just don't plan on keeping an accurate and up to date list of required libraries.)

After that, just run `dotnet build`.

## Enabling SkiaSharp for Scoreboard characters
Scoreboard characters mod uses SkiaSharp library ([GitHub](https://github.com/mono/SkiaSharp/), [main package NuGet](https://www.nuget.org/packages/SkiaSharp), [used native libs NuGet](https://www.nuget.org/packages/SkiaSharp.NativeAssets.Win32)) for image editing. Its DLLs (`SkiaSharp.dll`, `libSkiaSharp.dll`) should be placed in `UserLibs` folder of the modded game install. SkiaSharp is MIT-licensed, and I include these in downloads for convenience.

If you want to get these straight from NuGet instead, you can use `dotnet publish` command:
```bash
dotnet publish -c Release -r win-x64 --no-self-contained ScoreboardCharacters/ScoreboardCharacters.csproj
```
The SkiaSharp DLLs will be in `ScoreboardCharacters/bin/Release/net6.0/win-x64/publish` folder.

Why `win-x64` runtime? There is no native Muse Dash version for GNU/Linux, and I run the game via Proton, so the Windows binaries should work on both Windows and GNU/Linux for the time being. [Mac? No idea.](https://tenor.com/view/13786657)

You can also extract the DLLs from the nupkg files manually. Remember to look for `net6.0` and `win-x64` monikers.
