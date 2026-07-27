using MelonLoader;
using MelonLoader.Preferences;

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
                return 1f - ((1 - DimmingIntensity) * (1f - DefaultAlpha.Value));
            }
            LoggerInstance.Error("Default alpha not present, defaulting to full dim.");
            return 1f;
        }
    }

    private MelonPreferences_Category _prefsCategory;
    private MelonPreferences_Entry<float> _dimIntensity;

    internal float DimmingIntensity => _dimIntensity.Value;

    public override void OnInitializeMelon()
    {
        _prefsCategory = MelonPreferences.CreateCategory("Bnfour_SofterBackgroundDim");
        _dimIntensity = _prefsCategory.CreateEntry("Intensity", 0.5f,
            "Dim intensity", "Controls how much dimmer the effect background is. 0 is no dim at all, 1 is completely black.",
            validator: new ValueRange<float>(0f, 1f));
    }
}
