using HarmonyLib;
using Il2CppAssets.Scripts.GameCore.Managers;
using MelonLoader;

using Bnfour.MuseDashMods.ColorScoreStatus.Data;

namespace Bnfour.MuseDashMods.ColorScoreStatus.Patches;

/// <summary>
/// Downgrades the AP to FC on Yume's skill trigger, which is not reported as a
/// miss in the general judgement event patch. (Vanilla also does this.)
/// </summary>
[HarmonyPatch(typeof(SkillManager), nameof(SkillManager.YumeSkill))]
public class SkillManagerYumeSkillPatch
{
    internal static void Postfix()
    {
        var mod = Melon<ColorScoreStatusMod>.Instance;
        if (mod.Status == ComboStatus.AllPerfect)
        {
            mod.Status = ComboStatus.FullCombo;
        }
    }
}
