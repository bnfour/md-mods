using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;
using Il2CppUI.Controls.BtnTipStateControl;
using Il2CppPeroTools2.Resources;

using Bnfour.MuseDashMods.FeverSwitch.Components;
using MelonLoader;

namespace Bnfour.MuseDashMods.FeverSwitch.Patches;

/// <summary>
/// Updates the UI for the mod.
[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.Awake))]
public class PnlPreparationAwakePatch
{
    internal static void Postfix(PnlPreparation __instance)
    {
        // adjust the size for the toggle off image to fit the car image, which is a square
        if (__instance.transform.Find("RightRoot/Top/ImgRandomBg/ImgRandomFlagBg/ImgRandomFlag")
            ?.GetComponent<Image>()?.rectTransform is RectTransform rt)
        {
            rt.sizeDelta = new Vector2(42, 42);
        }
        // replace the U hint with F hint
        if (__instance.transform.Find("RightRoot/Top/ImgRandomBg/ImgRandomFlagBg/KeyTip/ImgRandomPCTip")
            ?.GetComponent<PnlLevelConfigRandomRoleKeyTipViewControl>() is PnlLevelConfigRandomRoleKeyTipViewControl control)
        {
            var provider = Melon<FeverSwitchMod>.Instance.SpriteProvider;
            if (provider != null)
            {
                control.m_ImgFront.sprite = provider.Hint;
            }
            else
            {
                Melon<FeverSwitchMod>.Logger.Warning("SpriteProvider not initialized, hint image will not be replaced");
            }
        }
        // add custom component to change the keybind, once or things will break
        if (__instance.transform.Find("Panels/PnlRankLocalization/Pc/PnlRank")?.gameObject is GameObject go
            && go.GetComponent<CustomFeverToggleKeybind>() == null)
        {
            go.AddComponent<CustomFeverToggleKeybind>();
        }
    }
}
