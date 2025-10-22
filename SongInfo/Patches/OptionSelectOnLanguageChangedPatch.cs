using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;

using Bnfour.MuseDashMods.SongInfo.Data;
using Bnfour.MuseDashMods.SongInfo.UI;
using Locale = Bnfour.MuseDashMods.SongInfo.Data.Locale;

namespace Bnfour.MuseDashMods.SongInfo.Patches;

/// <summary>
/// Updates the localizable text for the Best Record layout on language change.
/// </summary>
[HarmonyPatch(typeof(OptionSelect), nameof(OptionSelect.OnLanguageChanged))]
public class OptionSelectOnLanguageChangedPatch
{
    private static void Postfix()
    {
        if (Melon<SongInfoMod>.Instance.Layout == SongInfoLayout.BestRecord)
        {
            var durationHeader = GameObject.Find($"UI/Standerd/PnlPreparation/Panels/PnlRecord/{Constants.BestRecordPanel.DurationPath}");
            if (durationHeader != null)
            {
                durationHeader.GetComponent<Text>().text = Locale.Length;
            }
        }
    }
}
