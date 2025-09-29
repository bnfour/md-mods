using UnityEngine;

using Il2Cpp;

using Bnfour.MuseDashMods.SongInfo.UI;

namespace Bnfour.MuseDashMods.SongInfo.Utilities.UI.Setting;

internal sealed class TopRightSetterOneLine : TopRightSetterBase, IDataSetter
{
    protected override void FillText(Transform componentRoot, string bpm, string duration)
    {
        componentRoot?.Find(Constants.TopRight.OneLine)
            ?.GetComponent<LongSongNameController>()
            ?.Refresh($"{duration}, {bpm} BPM", delay: 0);
    }
}
