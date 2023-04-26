Unofficial quality of life modifications for the PC version of the hit video game Muse Dash using MelonLoader **0.5.7**

# Disclaimers
* These mods are unofficial and are not associated with, related to, and/or endorsed by peropero, hasuhasu, and/or XD.
* USE AT YOUR OWN RISK. NO WARRANTIES.
* Please read [FAQ](#frequently-asked-questions) and ["Known Issues"](#known-issues).

# Mod list
Currently, this repo contains two scoreboard-related mods.

They can be used together. Compatibility with other mods is unknown -- probably should work if other mods do not mess with the scoreboard itself and character selection too much. No guarantees though.

## Scoreboard characters
Mod file: `ScoreboardCharacters.dll`

This mod adds buttons to show charater and elfin used to obtain the score to the in-game scoreboard:

![earlier in the development there were numerical IDs, so that's an improvement](readme-images/scoreboard-characters.png)

These can be clicked, and will set your current character and elfin to these. It will even scroll the selection screens for you.

### Image overload
If you don't like the provided default images on the buttons (my quick and dirty cropped screenshots of the selection screen; devs please let me know if this is an infrigiment before firing a complaint), they can be changed.

To override the default builtin image, you need to place a 960×960 PNG named `scoreboard_characters_override.png` to `MuseDash_Data` directory of the game install. Individual sprites are 120×120, see `ScoreboardCharacters/Resources/sprites.png` for aligning them to the spritesheet.  
The override will silently fail if any of these differ from expected values. If you don't see your custom images, please double-check the file name, location, and resolution.

## True rank
Mod file: `TrueAbove1kRank.dll`

This small mod changes mysterious "999+" text in your own scoreboard entry to your actual rank if it lies within 1000--2000 range:

![simulated image, no (you)s were harmed during production](readme-images/true-rank-showcase.png)

Please note that the game servers only track top 2000 entries. If you score less, it won't be tracked and there's nothing this mod can do.

# Installation
These are [MelonLoader](https://melonwiki.xyz/) mods. In order to run these, you need to have it installed. **Only 0.5.7 version of MelonLoader is supported**, absolutely no idea about compatibility with newer versions.  
Once you have MelonLoader installed, drop the DLLs of desired mods into mods folder. Remove to uninstall.

Rather than downloading these, I suggest (reviewing the source and) building them yourself -- this way you'll be sure the mods behave as described. See ["Building from source"](#building-from-source).  
Otherwise, please verify the downloads via checksums provided for every release.

## Verification
Every version to be released will be accompanied with SHA256 hashes of every DLL. MelonLoader do print these in console when loading mods, but I suggest to verify hashes before running any code.

# Frequently Asked Questions
(or, more accurately, "I thought you may want to know this")
### Is this cheating?
_tl;dr: most likely not, depends on your definition_

These mods provide with the information the game already receives directly from its API, it's just not shown anywhere by default. You can already get this info, for instance, from [musedash.moe](https://musedash.moe/) scoreboard. In fact, this repo is born from my frustration of having to mirror my track selection in-game and on the website on another screen (and forgetting that second character in "YInMn Blue" is an uppercase I and not a lowercase L. Every single time).  
The mods provide no advantage in the actual gameplay, only some convinence in track selection stage. You still have to git gud to get high scores; you just know what character to pick.

Unless you count _any_ intervention for _any_ purpose with the game as cheating, this is not cheating.

### Will I get banned for using these?
_tl;dr: probably not, but NO WARRANTIES; USE AT YOUR OWN RISK and don't blame me_

As I stated in previous question, I don't believe this is cheating. I've been using these in various in-development stages for a few months, and my account is still there. But there's a reason for the all-caps section of the license about having no warranties: the devs might think otherwise or break the compatibility unintentionally.  

Remember that you're using the mods **at your own risk**. I warned you many times in this readme.

### My game broke because of you, how can I blame you?
_tl;dr: read known issues section first, and remember: NO WARRANTIES_

Please make sure you're using supported **(0.5.7)** version of MelonLoader.

There are some issues I do know about, see ["Known Issues"](#known-issues). Please do not report these, I'm well aware of them.  
There might be (and probably are) issues I do not know about, feel free to report these and we'll see what can be done.

# Known issues
I play the game on GNU/Linux, so some of these may be attributed to Proton/Wine shenanigans rather than shenanigans of Unity, game itself, or dotnet runtime. If you're not experiencing these, good for you.

## Scoreboard characters
### The buttons may not work on first click
_Workaround: doubleclick them ¯\\\_(ツ)\_/¯_

For whatever reason, for me, buttons (not just these added by the mod) do not respond to the first click on the screen. The issue is not that noticeable because everything else also has keyboard controls. Clicking once anywhere seems to fix the issue for the current menu.

### The scoreboard may not be fully populated
_Workaround: try to refresh [F5] if there are less than 99 entries_

For whatever reason, the scoreboard data is rarely appears to be cut short at the time of populating the custom buttons. Refresing the scoreboard _usually_ works. It didn't for me only once so far.  
Please note that the scoreboard might actually have less than 99 entries, if the track was relased a few hours ago.

### The heart symbol of selected character or elfin may linger for a few frames after leaving the selection screen
_Workaround: bear with it ¯\\\_(ツ)\_/¯_

Absolutely no data on this one. It's just there, no idea why.

### The scoreboard images may have slight artifacts
_Workaround: bear with it ¯\\\_(ツ)\_/¯_

I'm too lazy too investigate what exactly in the long pipeline of  
`image embedded in DLL -> in-mod magic -> Unity magic -> image on the screen`  
actually creates these. Sorry. It's not that noticeable anyway, I hope.

## True rank
None, unless something that I think comes from Scoreboard characters actually comes from here. Then again, this one is way more simplier.

# Building from source
This repo is a run-of-the-mill .NET solution targeting .NET 4.7.2.

The only gotcha is that some libraries required to build it are not included because of file size (and licensing) issues. Your installation of MelonLoader will generate them for you:
* Copy all files from `MelonLoader/Managed` folder from the game install to the `references` folder of this repo.
* Copy `MelonLoader.dll` from `MelonLoader` folder from the game install to the `references` folder of this repo.

This should cover the references in both mods.
