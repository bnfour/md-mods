using HarmonyLib;
using MelonLoader;

using Il2CppAssets.Scripts.Database;
using Il2CppPeroPeroGames.GlobalDefines;
using Il2CppAssets.Scripts.UI.Specials;

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
        // state is basically "are we switching from japanese?"
        __state = GlobalDataBase.s_DbUi.curLanguageIndex == Language.japanese;
    }

    internal static void Postfix(bool __state)
    {
        var logger = Melon<ScoreboardCharactersMod>.Logger;
        if (__state)
        {
            logger.Msg("switching FROM Japanese, move left");
        }
        // are we switching to japanese then?
        else if (GlobalDataBase.s_DbUi.curLanguageIndex == Language.japanese)
        {
            logger.Msg("switching TO Japanese, move right");
        }
    }
}
