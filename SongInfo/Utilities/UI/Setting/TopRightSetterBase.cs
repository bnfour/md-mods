using Il2Cpp;
using UnityEngine;

using Bnfour.MuseDashMods.SongInfo.Patches;

namespace Bnfour.MuseDashMods.SongInfo.Utilities.UI.Setting;

internal abstract class TopRightSetterBase : IDataSetter
{
    public void Set(PnlPreparation panel, string bpm, string duration)
    {
        var customObject = panel.transform.Find(Constants.TopRight.Component);

        FillText(customObject, bpm, duration);

        var animation = customObject.GetComponent<Animation>();
        animation?.Play(animation.clip?.name);
    }

    protected abstract void FillText(Transform componentRoot, string bpm, string duration);
}
