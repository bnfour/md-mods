using System;
using HarmonyLib;
using MelonLoader;

using Il2Cpp;

using Bnfour.MuseDashMods.SongInfo.Data;
using Bnfour.MuseDashMods.SongInfo.Utilities.UI.Generation;

namespace Bnfour.MuseDashMods.SongInfo.Patches;

/// <summary>
/// Patch to modify the UI of preparation panel depending on the selected layout.
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.Awake))]
public class PnlPreparationAwakePatch
{
    private static void Postfix(PnlPreparation __instance)
    {
        IComponentGenerator generator = Melon<SongInfoMod>.Instance.Layout switch
        {
            SongInfoLayout.OneLine => new TopRightGeneratorOneLine(),
            SongInfoLayout.TwoLines => new TopRightGeneratorTwoLines(),
            SongInfoLayout.BestRecord => new BestRecordPanelGenerator(),
            _ => throw new ApplicationException("Unknown layout type")
        };

        generator.CreateModUI(__instance);
    }
}
