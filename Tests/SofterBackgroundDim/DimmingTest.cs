using System.Reflection;
using Bnfour.MuseDashMods.SofterBackgroundDim;
using MelonLoader;

namespace Bnfour.MuseDashMods.Tests.SofterBackgroundDim;

public class DimmingTest
{
    private const float Eps = 0.0001f;

    private readonly SofterBackgroundDimMod _mod;
    private readonly FieldInfo _privatePrefs;

    private MelonPreferences_Entry<float> Pref => _privatePrefs.GetValue(_mod) as MelonPreferences_Entry<float>
        ?? throw new ApplicationException("Unable to locate field on mod instance");

    public DimmingTest()
    {
        _mod = new();
        _privatePrefs = typeof(SofterBackgroundDimMod).GetField("_dimIntensity", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new ApplicationException("Unable to reflect on the field");
        _privatePrefs.SetValue(_mod, new MelonPreferences_Entry<float>());
    }

    [Fact]
    public void FullDimWorks()
    {
        Pref.Value = 1f;

        // iterate over all vanilla _brightness_ percentages
        for (int i = 40; i <= 100; i++)
        {
            _mod.DefaultAlpha = 1f - ((float)i / 100);

            Assert.Equal(1, _mod.DimmedAlpha, Eps);
        }
    }
}
