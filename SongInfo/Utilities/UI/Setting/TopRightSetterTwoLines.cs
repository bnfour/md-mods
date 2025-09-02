using UnityEngine;

using Il2Cpp;

using Bnfour.MuseDashMods.SongInfo.Patches;

namespace Bnfour.MuseDashMods.SongInfo.Utilities.UI.Setting;

internal sealed class TopRightSetterTwoLines : TopRightSetterBase, IDataSetter
{
    protected override void FillText(Transform componentRoot, string bpm, string duration)
    {
        componentRoot?.Find(Constants.TopRight.TwoLinesBpm)
            ?.GetComponent<LongSongNameController>()
            ?.Refresh($"BPM: {bpm}", delay: 0);

        componentRoot?.Find(Constants.TopRight.TwoLinesDuration)
            ?.GetComponent<LongSongNameController>()
            ?.Refresh($"Length: {duration}", delay: 0);
    }
}
