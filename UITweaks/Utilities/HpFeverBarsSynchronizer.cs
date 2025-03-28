using System;
using System.IO;
using System.Reflection;
using UnityEngine;

using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.UITweaks.Data;

namespace Bnfour.MuseDashMods.UITweaks.Utilities;

/// <summary>
/// Utility class that actually modifies one of the HP/Fever bars in order to
/// sync their animations.
/// </summary>
internal static class HpFeverBarsSynchronizer
{
    private const string FillPathTemplate = "Below/UpUI/Sld{0}/Fill Area/Fill";
    private const string TexturePathTemplate = "Bnfour.MuseDashMods.UITweaks.Resources.bubbles.{0}.png";
    // this is the default horizontal "scale" for both bars' flow overlay
    // 4 means the whole bar will be filled by exactly 4 repetitions of the texture
    // TODO consider to also get this value at runtime like the widths
    private const float DefaultXScale = 4f;

    /// <summary>
    /// Actually syncs the bars by replacing the texture and rescaling the material
    /// and/or shader on one of them.
    /// </summary>
    /// <param name="panel">The <see cref="PnlBattleComps"/> instance to work on.</param>
    /// <param name="mode">Sets which bar to modify.</param>
    internal static void Sync(PnlBattleComps panel, HpFeverFlowSyncMode mode)
    {
        var targetFill = panel?.others?.transform?.Find(PathToModifiedComponent(mode));
        var referenceFill = panel?.others?.transform?.Find(PathToReferenceComponent(mode));

        if (targetFill != null && referenceFill != null)
        {
            var renderer = targetFill.gameObject.GetComponent<CanvasRenderer>();
            var material = renderer.GetMaterial();

            // the two bars' lengths ever so slightly differ (15 px on 1080),
            // yet both are set to be filled by exactly 4 repetitions of the bubble texture;
            // so we have to adjust the scale for one of the bars in order to sync their animations
            var targetWidth = targetFill.GetComponent<RectTransform>().rect.width;
            var referenceWidth = referenceFill.GetComponent<RectTransform>().rect.width;

            var adjustedScale = targetWidth / (referenceWidth / DefaultXScale);
            material.SetFloat("_FlowScaleX", adjustedScale);

            // yes, the heights are also different, but 2 px difference over 35% of texture
            // is not really noticeable;

            // i _really_ wanted to avoid replacing the texture outright,
            // but neither _FlowOffsetX float
            // nor material's offset for the flow texture worked, so here we go
            material.SetTexture("_FlowTex", GetReplacementTexture(mode));
        }
    }

    /// <summary>
    /// Returns the name of the bar to modify; to be templated into
    /// <see cref="FillPathTemplate"/>.
    /// </summary>
    private static string PathToModifiedComponent(HpFeverFlowSyncMode mode)
    {
        var part = mode switch
        {
            HpFeverFlowSyncMode.FeverToHp => "Power",
            HpFeverFlowSyncMode.HpToFever => "Hp",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), "Unsupported change mode")
        };

        return string.Format(FillPathTemplate, part);
    }

    /// <summary>
    /// Returns the name of the other bar to source scaling-related values from;
    /// to be templated into <see cref="FillPathTemplate"/>.
    /// </summary>
    private static string PathToReferenceComponent(HpFeverFlowSyncMode mode)
    {
        var part = mode switch
        {
            HpFeverFlowSyncMode.FeverToHp => "Hp",
            HpFeverFlowSyncMode.HpToFever => "Power",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), "Unsupported change mode")
        };

        return string.Format(FillPathTemplate, part);
    }

    /// <summary>
    /// Returns the offset texture to replace the original one, loaded from
    /// this assembly's resources.
    /// </summary>
    private static Texture2D GetReplacementTexture(HpFeverFlowSyncMode mode)
    {
        var part = mode switch
        {
            HpFeverFlowSyncMode.FeverToHp => "-116",
            HpFeverFlowSyncMode.HpToFever => "+118",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), "Unsupported change mode")
        };

        var resourcePath = string.Format(TexturePathTemplate, part);
        var assembly = typeof(HpFeverBarsSynchronizer).GetTypeInfo().Assembly;

        using (var textureStream = assembly.GetManifestResourceStream(resourcePath))
        using (MemoryStream memoryStream = new())
        {
            // MemoryStream is directly convertable to byte[]
            textureStream.CopyTo(memoryStream);
            // size is irrelevant, Alpha8 works and should save some memory,
            // and we want mipmap off, because the textures are 160x160 -- not powers of two
            var texture = new Texture2D(1, 1, TextureFormat.Alpha8, false);
            ImageConversion.LoadImage(texture, memoryStream.ToArray());

            return texture;
        }
    }
}
