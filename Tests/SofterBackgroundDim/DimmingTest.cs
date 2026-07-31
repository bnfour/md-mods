using System.Reflection;

using MelonLoader;

using Bnfour.MuseDashMods.SofterBackgroundDim;

namespace Bnfour.MuseDashMods.Tests.SofterBackgroundDim;

public class DimmingTest
{
    private const float Eps = 0.0001f;

    private readonly SofterBackgroundDimMod _mod;
    private readonly FieldInfo _privatePref;
    // convenience shortcut
    private MelonPreferences_Entry<float> Pref => _privatePref.GetValue(_mod) as MelonPreferences_Entry<float>
        ?? throw new ApplicationException("Unable to locate field on mod instance");

    public DimmingTest()
    {
        _mod = new();
        _privatePref = typeof(SofterBackgroundDimMod).GetField("_dimIntensity", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new ApplicationException("Unable to reflect on the field");
        _privatePref.SetValue(_mod, new MelonPreferences_Entry<float>());
    }

    [Fact]
    public void FullDimWorks()
    {
        Pref.Value = 1f;

        // iterate over all vanilla _brightness_ percentages
        for (int i = 40; i <= 100; i++)
        {
            _mod.DefaultAlpha = 1f - ((float)i / 100);

            Assert.Equal(1f, _mod.DimmedAlpha, Eps);
        }
    }

    [Fact]
    public void NoDimWorks()
    {
        Pref.Value = 0f;

        // iterate over all vanilla _brightness_ percentages
        for (int i = 40; i <= 100; i++)
        {
            _mod.DefaultAlpha = 1f - ((float)i / 100);

            Assert.Equal(_mod.DefaultAlpha.Value, _mod.DimmedAlpha, Eps);
        }
    }

    [Theory]
    // how it actually intended to work,
    // 50% intensity on 40% brightness = 20% effective brightness
    [InlineData(0.6f, 0.5f, 0.8f)]
    // other random stuff:
    // 50% intensity on 50% brightness = 25% effective brightness
    [InlineData(0.5f, 0.5f, 0.75f)]
    // 10% intensity on 90% brightness = 81% effective brightness
    [InlineData(0.1f, 0.1f, 0.19f)]
    // 30% intensity on 100% brightness = 70% effective brightness
    [InlineData(0f, 0.3f, 0.3f)]
    public void ArbitraryValuesTest(float defaultAlpha, float intensity, float expectedDimAlpha)
    {
        Pref.Value = intensity;
        _mod.DefaultAlpha = defaultAlpha;

        Assert.Equal(expectedDimAlpha, _mod.DimmedAlpha, Eps);
    }
}
