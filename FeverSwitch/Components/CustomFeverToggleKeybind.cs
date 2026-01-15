using MelonLoader;
using UnityEngine;

using Il2CppAssets.Scripts.UI.Panels;

namespace Bnfour.MuseDashMods.FeverSwitch.Components;

/// <summary>
/// Very specialized keybinding component.
/// The only use is to call <see cref="PnlRank.OnClickRandom"/>
/// from a sibling <see cref="PnlRank"/> on F key press.
/// </summary>
[RegisterTypeInIl2Cpp]
internal class CustomFeverToggleKeybind(IntPtr pointer) : MonoBehaviour(pointer)
{
    internal void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // this method is patched and is not actually executed,
            // but we don't care about that here
            transform.GetComponent<PnlRank>()?.OnClickRandom();
        }
    }
}
