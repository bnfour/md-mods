using HarmonyLib;

using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Specials;
using Il2CppPeroPeroGames.GlobalDefines;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

/// <summary>
/// Applies extra offset to the moved UI when switching to/from Japanese.
/// (see #34)
/// </summary>
[HarmonyPatch(typeof(SwitchLanguages), nameof(SwitchLanguages.OnClick))]
public class SwitchLanguagesOnClickPatch
{
    internal static void Prefix(out bool __state)
    {
        // state is basically "are we switching _from_ japanese?"
        __state = GlobalDataBase.s_DbUi.curLanguageIndex == Language.japanese;
    }

    internal static void Postfix(bool __state)
    {
        if (__state)
        {
            UiPatcher.ApplyLocaleSpecificOffset(-1);
        }
        // have we switched _to_ japanese then?
        else if (GlobalDataBase.s_DbUi.curLanguageIndex == Language.japanese)
        {
            UiPatcher.ApplyLocaleSpecificOffset(1);
        }
    }
}
