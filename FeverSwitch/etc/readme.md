# et cetera
This folder contains original vector sources for the original images included in the mod DLL as PNG "renders".
Here to not lose them, as they originally were in my `~` folder, and also for repo transparency — feel free to improve them.

## Technical details
The non-golden car sprite is wider than the default cube (42 compared to 38), so the image itself is resized in the code.
All other sprites match the original size in pixels (when "rendered" as PNGs, see the [actual resources folder](../Resources/))

## Sources
The original icons for hand and car emojis are from [Twemoji](https://github.com/twitter/twemoji); available under [CC-BY-4.0](https://github.com/twitter/twemoji/blob/master/LICENSE-GRAPHICS).

The <kbd>U</kbd> hint reference is from the game files.

## [`PcButtonF`](./PcButtonF.svg)
The game does contain a sprite for <kbd>F</kbd> hint (`ButtonF` without the "PC" part), but its proportions differ from `PcButtonU` I'm replacing, so I had to make one with correct size of the letter.

The result is this file, complete with the reference and a dark background layer for visibility in Inkscape.

![](./PcButtonF.svg)

## Toggle images
All other SVG in this folder are used (or unused) for the fever mode display — manual and auto.

Yeah, I couldn't think of anything better than a car (**auto**mobile) for the auto fever mode, but I kinda grew on it. Hope it makes you smile as well. Why is the hand left? Because I'm left-handed; the more you know.

### Descriptions
- [`1f698.svg`](./1f698.svg) and [`1f590.svg`](./1f590.svg) are source SVGs straight from the Twemoji repo.
- The "_changed" versions are for the "auto is default" setting, which is the default:
- - [The car](./1f698_changed.svg) is monochrome with simplified paths; this image is also used as base for other changes.
- - [The hand](./1f590_changed.svg) is styled to match pre 5.10.0 `BtnStochastic` sprite for the on toggle; no longer relevant.
- The "_changed_alt" versions are for the "manual is default" alternate settings mode:
- - [The car](./1f698_changed_alt.svg) is styled to match pre 5.10.0 `BtnStochastic` sprite for the on toggle; no longer relevant.
- - [The hand](./1f590_changed_alt.svg) is monochrome and mirrored; this image is also used as base for other changes.
- [`gold_medal_sources.svg`](./gold_medal_sources.svg) is the source for post 5.10.0 toggle on sprites; the background is shared, the car and icons are in different layers.

| kind | src | changed | changed_alt (unused) |
| --- | --- | --- | --- |
| car | ![](./1f698.svg) | ![](./1f698_changed.svg) | ![](./1f698_changed_alt.svg) |
| hand | ![](./1f590.svg) | ![](./1f590_changed.svg) | ![](./1f590_changed_alt.svg) |

If you read all this, here's a medal for you:

![](./gold_medal_sources.svg)
