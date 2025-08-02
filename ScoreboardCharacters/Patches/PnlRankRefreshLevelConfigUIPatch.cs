using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.RefreshLevelConfigUI))]
public class PnlRankRefreshLevelConfigUIPatch
{
    // TODO actual name
    private const string customComponentPath = "UI/Standerd/PnlPreparation/RightRoot/Top/RootLevelConfigShow/SomeNameOne";

    internal static void Postfix()
    {
        var image = GameObject.Find(customComponentPath)?.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = Melon<ScoreboardCharactersMod>.Instance.ButtonImageProvider.GetSprite
            (
                (Character)DataHelper.selectedRoleIndex,
                (Elfin)DataHelper.selectedElfinIndex
            );
        }
    }
}
