using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

using Il2CppAssets.Scripts.UI.Panels;
using Il2CppAssets.Scripts.Database;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.RefreshLevelConfigUi))]
public class PnlRankRefreshLevelConfigUiPatch
{
    internal static void Postfix(PnlRank __instance)
    {
        // TODO very wip

        // update the character/elfin custom image
        var image = GameObject.Find("BnTopLevelConfigState")
            .GetComponent<Image>();

        var provider = Melon<ScoreboardCharactersMod>.Instance.ButtonImageProvider;

        var levelCharacter = (Character)DataHelper.selectedRoleIndex;
        var levelElfin = (Elfin)DataHelper.selectedElfinIndex;

        image.sprite = provider.GetSprite(levelCharacter, levelElfin);

        // move the self rank to its position if scoreboard is expanded
        if (__instance.isRankExpand)
        {
            var selfRankRectTransform = __instance.server.GetComponent<RectTransform>();
            selfRankRectTransform.anchoredPosition3D = new Vector3
            (
                selfRankRectTransform.anchoredPosition3D.x,
                // TODO tweak value
                selfRankRectTransform.anchoredPosition3D.y - 75,
                selfRankRectTransform.anchoredPosition3D.z
            );
        }
    }
}
