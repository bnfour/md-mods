using HarmonyLib;

using Il2CppAssets.Scripts.UI.Panels;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.UITweaks.Patches;

/// <summary>
/// Formats the score values to separate thousands with a space, allegedly for
/// an even easier read.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.UIRefresh))]
public class ScoreThousandsSeparator
{
    internal static void Postfix(PnlRank __instance)
    {
        // TODO make it optional

        // TODO is it worth moving from here to an utility class?
        var formatter = (int score) => score.ToString("N0").Replace(',', ' ');

        if (__instance.txtServerScore.IsActive()
            && int.TryParse(__instance.txtServerScore.text, out int ourScore))
        {
            __instance.txtServerScore.text = formatter(ourScore);
        }
        // see Scoreboard characters' PnlRank.UIRefresh patch to learn why it's
        // iterated that way
        for (int i = __instance.m_RankPool.gameObjects.Count - 1; i >= 0; i--)
        {
            var entry = __instance.m_RankPool.gameObjects[i];
            // stop if an entry is currently unused — the following ones are too
            if (!entry.active)
            {
                break;
            }

            if (entry.transform.FindChild("TxtScoreValueS")?.GetComponent<Text>() is Text textComponent
                && int.TryParse(textComponent.text, out int score))
            {
                textComponent.text = formatter(score);
            }
        }
    }
}
