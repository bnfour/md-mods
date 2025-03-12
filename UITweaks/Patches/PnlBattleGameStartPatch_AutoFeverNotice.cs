using HarmonyLib;
using MelonLoader;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

using Il2CppAssets.Scripts.UI.Panels;
using Il2CppAssets.Scripts.Database;
using Il2CppPeroPeroGames.GlobalDefines;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Patch that replaces the "FEVER" sprite on the fever bar with "AUTO"
/// if configured to and needed. 
/// </summary>
[HarmonyPatch(typeof(PnlBattle), nameof(PnlBattle.GameStart))]
public class PnlBattleGameStartPatch_AutoFeverNotice
{
    private const string ReplacementImagePathTemplate = "Bnfour.MuseDashMods.UITweaks.Resources.auto.{0}.png";

    private static void Postfix(PnlBattle __instance)
    {
        // first check if the feature is enabled at all,
        // peropero aniki ranbu has no ui,
        // touhou mode uses different sprite and has no fever,
        // finally check if the sprite needs to be changed
        if (!Melon<UITweaksMod>.Instance.AutoFeverNoticeEnabled
            || GlobalDataBase.dbBattleStage.musicUid == MusicUidDefine.peropero_aniki_ranbu
            || GlobalDataBase.s_DbTouhou.isTouhouEasterEgg
            || !DataHelper.isAutoFever)
        {
            return;
        }

        // component to replace the sprite in
        var image = __instance.currentComps.others.transform.Find("Below/UpUI/ImgFever")?.GetComponent<Image>();

        var selector = GlobalDataBase.dbBattleStage.musicUid == MusicUidDefine.bad_apple
            ? "badapple" : "default";
        var path = string.Format(ReplacementImagePathTemplate, selector);

        var assembly = typeof(PnlBattleGameStartPatch_AutoFeverNotice).GetTypeInfo().Assembly;

        using (var textureStream = assembly.GetManifestResourceStream(path))
        using (MemoryStream memoryStream = new())
        {
            // MemoryStream is directly convertable to byte[]
            textureStream.CopyTo(memoryStream);
            // no mipmap, as usual
            var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            ImageConversion.LoadImage(texture, memoryStream.ToArray());

            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
