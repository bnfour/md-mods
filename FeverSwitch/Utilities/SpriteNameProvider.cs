using System.Diagnostics;

using Il2Cpp;
using Il2CppPeroTools2.Resources;

namespace Bnfour.MuseDashMods.FeverSwitch.Utilities;

/// <summary>
/// Loads the names of the sprites to replace from the game's resources file
/// when needed.
/// </summary>
internal class SpriteNameProvider
{
    // backing fields for laziness
    private string? _randomOffSpriteName;
    private string? _randomOnSpriteName;

    internal string RandomOffSpriteName
        => _randomOffSpriteName ?? LoadSpriteNamesFromOffSpriteName();
    internal string RandomOnSpriteName
        => _randomOnSpriteName ?? LoadSpriteNamesFromOnSpriteName();


    // i only want to load the GlobalScriptableObjectData only once on first access
    // to any of the two properties, so the code is a bit messy

    private void LoadSpriteNamesFromResources()
    {
        var data = ResourcesManager.instance.LoadFromName<GlobalScriptableObjectData>("GlobalScriptableObjectData");

        _randomOffSpriteName = data.imgRandomOffName;
        _randomOnSpriteName = data.imgRandomOnName;
    }

    private string LoadSpriteNamesFromOffSpriteName()
    {
        LoadSpriteNamesFromResources();
        Debug.Assert(!string.IsNullOrEmpty(_randomOffSpriteName), "null imgRandomOffName after load");

        return _randomOffSpriteName;
    }

    private string LoadSpriteNamesFromOnSpriteName()
    {
        LoadSpriteNamesFromResources();
        Debug.Assert(!string.IsNullOrEmpty(_randomOnSpriteName), "null imgRandomOnName after load");

        return _randomOnSpriteName;
    }
}
