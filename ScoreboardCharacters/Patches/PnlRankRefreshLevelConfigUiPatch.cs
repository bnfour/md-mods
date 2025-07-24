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
        var image = GameObject.Find(UiPatcher.NewConfigUiComponentName)?.GetComponent<Image>();
        if (image != null)
        {
            var provider = Melon<ScoreboardCharactersMod>.Instance.ButtonImageProvider;

            image.sprite = DataHelper.isUseRandomLevelConfig
                ? provider.GetRandomSprite()
                : provider.GetSprite((Character)DataHelper.selectedRoleIndex, (Elfin)DataHelper.selectedElfinIndex);
        }

        if (__instance != null)
        {
            // move the self rank to its custom position whenever the scoreboard is expanded or collapsed
            Vector2 delta = __instance.isRankExpand ? new(0, -75.5f) : new(0, 3);
            UiPatcher.Move(__instance.server.transform, delta);

            // prevent the UI for big character selector blocking clicks for expanded scoreboard when random mode is on (see #21),
            // restore interactivity when scoreboard is collapsed and the UI in question is shown
            var rightLevelConfigGroup = __instance.transform.Find("Mask/rootBtnLevelConfigRight")?.GetComponent<CanvasGroup>();
            if (rightLevelConfigGroup != null)
            {
                rightLevelConfigGroup.blocksRaycasts = !__instance.isRankExpand;
                rightLevelConfigGroup.interactable = !__instance.isRankExpand;
            }
        }
    }
}
