using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

using Il2CppAssets.Scripts.UI.Panels;
using Il2CppAssets.Scripts.Database;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

[HarmonyPatch(typeof(PnlRank), "RefreshLevelConfigUi")]
public class PnlRankRefreshLevelConfigUiPatch
{
    internal static void Postfix(PnlRank __instance)
    {
        // TODO very wip
        var image = GameObject.Find("BnTopLevelConfigState")
            .GetComponent<Image>();

        var provider = Melon<ScoreboardCharactersMod>.Instance.ButtonImageProvider;

        var levelCharacter = (Character)DataHelper.selectedRoleIndex;
        var levelElfin = (Elfin)DataHelper.selectedElfinIndex;

        image.sprite = provider.GetSprite(levelCharacter, levelElfin);
    }
}
