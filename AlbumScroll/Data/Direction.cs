namespace Bnfour.MuseDashMods.AlbumScroll.Data;

/// <summary>
/// Used to store typed info about the direction of scroll this mod is affecting.
/// </summary>
public enum Direction
{
    /// <summary>
    /// Previous song: A, left arrow key, mouse scroll up, left on screen button.
    /// </summary>
    Backward,
    /// <summary>
    /// Next song: D, right arrow key, mouse scroll down, right on screen button.
    /// </summary>
    Forward
}
