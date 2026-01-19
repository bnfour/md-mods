using MelonLoader;
using UnityEngine;

namespace Bnfour.MuseDashMods.AlbumScroll.Components;

/// <summary>
/// Component that keeps track of the state of shift keys.
/// Intended to be attached to the songs' FancyScrollView.
/// </summary>
[RegisterTypeInIl2Cpp]
internal class ShiftStateTracker(System.IntPtr pointer) : MonoBehaviour(pointer)
{
    private bool _leftShiftDown;
    private bool _rightShiftDown;

    internal bool ShiftDown => _leftShiftDown || _rightShiftDown;

    internal void OnDisable()
    {
        _leftShiftDown = false;
        _rightShiftDown = false;
    }

    internal void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _leftShiftDown = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _leftShiftDown = false;
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            _rightShiftDown = true;
        }
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            _rightShiftDown = false;
        }
    }
}
