using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

using Il2Cpp;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// A patch to force all-caps text for option menu buttons.
/// </summary>
[HarmonyPatch]
public class OptionSelectPatch_UpperCase
{
    private static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(OptionSelect), nameof(OptionSelect.OnAwake));
        // for a not very common case when the language is switched to English
        // from another one after menu was initialized
        yield return AccessTools.Method(typeof(OptionSelect), nameof(OptionSelect.OnLanguageChanged));
    }

    private static void Postfix(OptionSelect __instance)
    {
        // TODO consider checking for the language
        // currently fires for all languages, but text case only makes sense for English
        if (!Melon<UITweaksMod>.Instance.FullCapsForOptionButtons)
        {
            return;
        }

        foreach (var kvp in __instance.m_SelectedButtonList)
        {
            var textComponent = kvp.Value?.GetComponentInChildren<Text>();
            textComponent?.text = textComponent.text.ToUpper();
        }
    }
}
