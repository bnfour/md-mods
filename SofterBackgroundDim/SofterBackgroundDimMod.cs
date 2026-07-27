using MelonLoader;

namespace Bnfour.MuseDashMods.SofterBackgroundDim;

public class SofterBackgroundDimMod : MelonMod
{
    /// <summary>
    /// Alpha of the black rectangle over the scene background used to change
    /// its perceived brightness in default condition. Used both to calculate a
    /// dimmed value, and to restore it later on effect end.
    /// </summary>
    /// <remarks>
    /// Inversely proportional to the brightness — 100% brightness is 0 alpha
    /// (the rect is not visible), 0% brightness is 1 alpha, and so on; lowest
    /// vanilla 40% is 0.6 alpha.
    /// </remarks>
    internal float? DefaultAlpha { get; set; }

    // TODO actual calculations, hardcoded to 20% right now
    /// <summary>
    /// Alpha of the black rectangle when the background is additionally dimmed.
    /// </summary>
    /// <remarks>
    /// While we store ready to use alphas, all calculations are carried in
    /// brightness space, hence all those "1 -".
    /// </remarks>
    internal float DimmedAlpha => 0.8f;

    // TODO configuration
}
