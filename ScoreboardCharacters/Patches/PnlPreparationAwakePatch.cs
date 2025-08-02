using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

using Il2Cpp;
using Il2CppInterop.Runtime;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

[HarmonyPatch(typeof(PnlPreparation), nameof(PnlPreparation.Awake))]
public class PnlPreparationAwakePatch
{
    internal static void Postfix(PnlPreparation __instance)
    {
        // TODO null checks everywhere
        // TODO move actual code back to UIPatcher?
        var levelConfigUIGroup = __instance.transform.Find("RightRoot/Top");
        if (levelConfigUIGroup != null)
        {
            // move entire group
            levelConfigUIGroup.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(-1180, -214, 0);

            // add custom image as a component in a custom object,
            // set to neutral positioning/scaling -- everything is done in the image component
            // TODO actual name
            var holderObject = new GameObject("SomeNameOne", Il2CppType.Of<Image>());
            holderObject.transform.parent = levelConfigUIGroup.Find("RootLevelConfigShow").transform;
            holderObject.transform.position = new(0, 0, 0);
            holderObject.transform.localScale = new(1, 1, 1);

            var currentConfigImage = holderObject.GetComponent<Image>();
            // 1x scale results in the image being 64x32 on 1080
            currentConfigImage.rectTransform.localScale = new(1.25f, 1.25f, 1);
            currentConfigImage.rectTransform.sizeDelta = new(80, 40);
            // just to the right of config lock button, taking scale and pixel perfectish offsets into account:
            // 43 is amount of _screen_ pixels (in 1080) to move,
            // offsets are to clamp position to whole pixels
            currentConfigImage.rectTransform.anchoredPosition3D = new(-43 * 1.25f + 0.215f, 0.3f, 0);
            // TODO white to not interfere with sprite
            currentConfigImage.color = Color.cyan;

            // hide default text

            // make component narrower
            // probably includes shrinking/replacing the bg image, moving E button hint, and moving the entire random toggle
        }

    }
}
