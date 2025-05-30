using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

using Il2CppAssets.Scripts.UI.Panels;
using Il2CppAssets.Scripts.Database;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;
using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

/// <summary>
/// A patch that updates the custom level config UI when vanilla one is updated
/// to keep display state consistent.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.RefreshLevelConfigUi))]
public class PnlRankRefreshLevelConfigUiPatch
{
    internal static void Postfix(PnlRank __instance)
    {
        // update the character/elfin custom image
        var image = GameObject.Find(UiPatcher.NewConfigUiComponentName).GetComponent<Image>();
        var provider = Melon<ScoreboardCharactersMod>.Instance.ButtonImageProvider;

        var levelCharacter = (Character)DataHelper.selectedRoleIndex;
        var levelElfin = (Elfin)DataHelper.selectedElfinIndex;

        image.sprite = provider.GetSprite(levelCharacter, levelElfin);

        // move the self rank to its position if scoreboard is expanded
        if (__instance.isRankExpand)
        {
            UiPatcher.Move(__instance.server.transform, new(0, -78.5f));
        }
    }
}
