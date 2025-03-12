using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;
using Il2CppPeroPeroGames.GlobalDefines;

namespace Bnfour.MuseDashMods.UITweaks.Utilities;

/// <summary>
/// Helper that replaces "FEVER" text on in game UI to "AUTO".
/// </summary>
internal static class FeverTextTextureReplacer
{
    private const string ReplacementImagePathTemplate = "Bnfour.MuseDashMods.UITweaks.Resources.auto.{0}.png";

    internal static void Replace(PnlBattle panel)
    {
        // component to replace the sprite in
        var image = panel.currentComps.others.transform.Find("Below/UpUI/ImgFever")?.GetComponent<Image>();

        if (image != null)
        {
            var selector = GlobalDataBase.dbBattleStage.musicUid == MusicUidDefine.bad_apple
                ? "badapple" : "default";
            var path = string.Format(ReplacementImagePathTemplate, selector);

            var assembly = typeof(FeverTextTextureReplacer).GetTypeInfo().Assembly;
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
}
