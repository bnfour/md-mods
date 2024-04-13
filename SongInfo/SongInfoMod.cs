using Bnfour.MuseDashMods.SongInfo.Utilities;
using MelonLoader;

namespace Bnfour.MuseDashMods.SongInfo;

public class SongInfoMod : MelonMod
{
    // TODO debug-only data generation mode
    
    public readonly SongDurationProvider DurationProvider = new();

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        DurationProvider.Shutdown();
    }
}
