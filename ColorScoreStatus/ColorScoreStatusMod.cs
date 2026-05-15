using System;
using MelonLoader;

using Bnfour.MuseDashMods.ColorScoreStatus.Data;
using Bnfour.MuseDashMods.ColorScoreStatus.Utilities.ColorSetting;

namespace Bnfour.MuseDashMods.ColorScoreStatus;

public class ColorScoreStatusMod : MelonMod
{
    internal ComboStatus Status
    {
        get => _status;
        set
        {
            // only status downgrades are allowed in-game,
            // reset to default AP is done separately as a part of ResetState()
            if ((int)value < (int)_status)
            {
                throw new InvalidOperationException("Unable to upgrade the combo status.");
            }

            _status = value;
        }
    }

    internal IScoreColorer scoreColorer;

    private ComboStatus _status;

    internal void ResetState()
    {
        _status = ComboStatus.AllPerfect;
        scoreColorer = null;
    }
}
