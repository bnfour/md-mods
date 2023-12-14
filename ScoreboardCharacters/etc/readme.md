# et cetera
This folder contains stuff that's somewhat related to development process, and is too specific to be placed elsewhere.

## Randomizer for screenshots
`randomizer-for-screenshots.patch` is a git patch that changes loading characters and elfins data from the API response to completely random values. It's used to create screenshots for readme, as the real scoreboards mostly contain few same characters and elfins over and over. If you're not me, this is probably useless.

After re-implementing this again after yet another rendering improvement, I thought it's a good idea to store it for future use.

Note that the hardcoded random maximums are up to date as of game version 3.11.0.

### Usage
- `git apply ScoreboardCharacters/etc/randomizer-for-screenshots.patch`
- build, copy to game's mod folder
- run the game, take screenshots
- discard the changes to `AdditionalScoreboardDataEntry.cs`
- put updated image to repo's readme

## Spritesheet source
`sprites.png` is the reference spritesheet.

It has 120×120 sprites -- three times the size needed for 1080 resolution. Only scaled down versions for common resolutions are included in the mod DLL -- see `ScoreboardCharacters/Resources` folder.

### Sprites placement
The spritesheet is 8 sprites in width: the first 5 columns are used for the character sprites, and the last 3 for the elfin sprites. Please note that elfins sprites include "no elfin" sprite (in-game id -1) before the elfin with id 0.
