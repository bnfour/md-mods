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
    /// The original formula in _brightness_ space was
    /// 1 - ((1 - Intensity) * (1 - Default)),
    /// where 1 - value is conversion between alpha and brightness.
    /// The code uses simplified equivalent expression.
    /// </remarks>
    internal float DimmedAlpha
    {
        get
        {
            if (DefaultAlpha.HasValue)
            {
                return DefaultAlpha.Value + DimmingIntensity - DefaultAlpha.Value * DimmingIntensity;
            }
            LoggerInstance.Error("Default alpha not present, defaulting to full dim.");
            return 1f;
        }
    }

    private MelonPreferences_Category _prefsCategory;
    private MelonPreferences_Entry<float> _dimIntensity;

    private float DimmingIntensity => _dimIntensity.Value;

    public override void OnInitializeMelon()
    {
        _prefsCategory = MelonPreferences.CreateCategory("Bnfour_SofterBackgroundDim");
        _dimIntensity = _prefsCategory.CreateEntry("Intensity", 0.5f,
            "Dim intensity", "Controls how much dimmer the effect background is. 0 is no dim at all, 1 is completely black.",
            validator: new ValueRange<float>(0f, 1f));
    }
}
