using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;
using Il2CppUI.Controls.BtnTipStateControl;
using Il2CppPeroTools2.Resources;

using Bnfour.MuseDashMods.FeverSwitch.Components;

namespace Bnfour.MuseDashMods.FeverSwitch.Patches;

/// <summary>
/// Updates the UI for the mod:<br/>
/// - resizes the random toggle off image to be a 42×42 square to fit the auto icon<br/>
/// - add a single copy of a custom keybinding component so that F for fever works
/// </summary>
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.Awake))]
public class PnlPreparationAwakePatch
{
    internal static void Postfix(PnlPreparation __instance)
    {
        if (__instance.transform.Find("RightRoot/Top/ImgRandomBg/ImgRandomFlagBg/ImgRandomFlag")
            ?.GetComponent<Image>()?.rectTransform is RectTransform rt)
        {
            rt.sizeDelta = new Vector2(42, 42);
        }

        if (__instance.transform.Find("RightRoot/Top/ImgRandomBg/ImgRandomFlagBg/KeyTip/ImgRandomPCTip")
            ?.GetComponent<PnlLevelConfigRandomRoleKeyTipViewControl>() is PnlLevelConfigRandomRoleKeyTipViewControl control)
        {
            // TODO custom F sprite with PcButtonU proportions
            control.m_ImgFront.sprite = ResourcesManager.instance.LoadFromName<Sprite>("ButtonF");
        }
        // add custom component to change the keybind, once or things will break
        if (__instance.transform.Find("Panels/PnlRankLocalization/Pc/PnlRank")?.gameObject is GameObject go
            && go.GetComponent<CustomFeverToggleKeybind>() == null)
        {
            go.AddComponent<CustomFeverToggleKeybind>();
        }
    }
}
