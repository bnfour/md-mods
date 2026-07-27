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
    internal float DimmedAlpha
    {
        get
        {
            if (DefaultAlpha.HasValue)
            {
                // convert alpha to brightness, dim it by multiplier,
                // convert resulting brightness back to alpha
                return 1f - (DimmingIntensity * (1f - DefaultAlpha.Value));
            }
            LoggerInstance.Error("Default alpha not present, defaulting to full dim.");
            return 1f;
        }
    }

    // TODO configuration, supposed to be in [0, 1] range
    internal const float DimmingIntensity = 0.5f;
}
