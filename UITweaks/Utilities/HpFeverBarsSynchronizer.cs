using System;
using UnityEngine;
using MelonLoader;

using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.UITweaks.Data;

namespace Bnfour.MuseDashMods.UITweaks.Utilities;

internal static class HpFeverBarsSynchronizer
{
    private const string FillPathTemplate = "Below/UpUI/Sld{0}/Fill Area/Fill";
    // TODO consider to also get this value at runtime like the widths
    private const float DefaultXScale = 4f;

    internal static void Sync(PnlBattleComps panel, HpFeverFlowSyncMode mode)
    {
        var targetFill = panel.others.transform.Find(PathToModifiedComponent(mode));
        var referenceFill = panel.others.transform.Find(PathToReferenceComponent(mode));

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
            // the bubbles are also not exact circles due to separate H and V scaling,
            // the more you know...

            // TODO set the texture
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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
    /// 
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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
}
