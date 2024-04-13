using MelonLoader;

using Bnfour.MuseDashMods.SongInfo.Utilities;

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
