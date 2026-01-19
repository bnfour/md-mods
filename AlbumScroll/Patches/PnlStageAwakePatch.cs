using HarmonyLib;

using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.AlbumScroll.Components;

namespace Bnfour.MuseDashMods.AlbumScroll.Patches;

/// <summary>
/// Adds the custom component to track shift keys state to the song scroll view.
/// </summary>
[HarmonyPatch(typeof(PnlStage), nameof(PnlStage.Awake))]
public class PnlStageAwakePatch
{
    internal static void Postfix(PnlStage __instance)
    {
        var fsv = __instance.transform.Find("StageUi/MusicRoot/FancyScrollViewMusic");
        if (fsv != null && fsv.gameObject.GetComponent<ShiftStateTracker>() == null)
        {
            fsv.gameObject.AddComponent<ShiftStateTracker>();
        }
    }
}
