using System.Collections.Generic;
using MelonLoader;
using HarmonyLib;
using Newtonsoft.Json;

using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;
using Bnfour.MuseDashMods.ScoreboardCharacters.Data.Api;
using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches;

/// <summary>
/// A patch to extract additional data (character and elfin ids)
/// from the full API response and display it as part of the scoreboard.
/// </summary>
[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.UIRefresh))]
public class PnlRankUIRefreshPatch
{
    /// <summary>
    /// Prefix method to populate additional data to display from full API response.
    /// </summary>
    /// <param name="uid">Song unique id, used to get data from the scoreboard.</param>
    /// <param name="__instance">The instance of <see cref="PnlRank"/> to patch UI in.</param>
    /// <param name="__state">Additional scoreboard data to pass to <see cref="Postfix"/>.</param>
    private static void Prefix(string uid, PnlRank __instance, out AdditionalScoreboardData __state)
    {
        __state = new();

        if (__instance.m_SelfRank.ContainsKey(uid))
        {
            SelfRank selfRank;
            try
            {
                // this is an extremely blunt way to do this,
                // but I didn't find a way to nicely convert
                // il2cpp's JToken to a managed object
                // TODO consider better conversion
                selfRank = JsonConvert.DeserializeObject<SelfRank>(__instance.m_SelfRank[uid].ToString());
            }
            // TODO catch jsonconvert's specific exceptions only?
            catch
            {
                Melon<ScoreboardCharactersMod>.Logger.Error("Unable to deserialize self-rank data!");
                selfRank = null;
            }

            if (selfRank?.Info != null)
            {
                __state.Self = new AdditionalScoreboardDataEntry(selfRank.Info);
            }
        }

        if (__instance.m_Ranks.ContainsKey(uid))
        {
            List<ScoreboardEntry> scoreboard;
            try
            {
                // same scuffed "render to a string, then use a proper library to get only relevant data" approach
                scoreboard = JsonConvert.DeserializeObject<List<ScoreboardEntry>>(__instance.m_Ranks[uid].ToString());
            }
            catch
            {
                Melon<ScoreboardCharactersMod>.Logger.Error("Unable to deserialize scoreboard data!");
                scoreboard = null;
            }

            if (scoreboard == null)
            {
                return;
            }

            foreach (var entry in scoreboard)
            {
                var data = new AdditionalScoreboardDataEntry(entry.Info);
                __state.Scoreboard.Add(data);
            }
        }
    }

    /// <summary>
    /// Postfix method to actually display the data gathered in <see cref="Prefix"/> in the UI.
    /// </summary>
    /// <param name="__instance">The instance of <see cref="PnlRank"/> to patch UI in.</param>
    /// <param name="__state">Scoreboard data to display, filled in <see cref="Prefix"/>.</param>
    private static void Postfix(PnlRank __instance, AdditionalScoreboardData __state)
    {
        // self-rank is handled separately
        if (__instance.server.active)
        {
            UiPatcher.FillScoreboardEntry(__instance.server, __state.Self);
        }
        // the scoreboard itself is pooled, there are 100 objects in the pool,
        // and the rank objects are in reverse order:
        //   - #0 is _mostly_ unused (_sometimes_ #100 entry),
        //   - #1 is the usual last scoreboard entry (#99),
        //   - #2 is the second last scoreboard entry (#98),
        //   ...and so on...
        //   - last entry is the first scoreboard entry for the top score
        // so if the scoreboard is not full, objects from the _beginning_ of the pool
        // are inactive
        var poolCount = __instance.m_RankPool.gameObjects.Count;
        for (int i = poolCount - 1; i >= 0; i--)
        {
            var actualEntry = __instance.m_RankPool.gameObjects[i];
            // if it's not active (not shown on the scoreboard),
            // we should silently skip filling from it onwards
            // (most likely, there are less entries than there are places in the scoreboard,
            // in particular, #100 is rarely used (is it an API bug?))
            if (!actualEntry.active)
            {
                break;
            }

            // unlike the objects pool, scoreboard data is stored simply:
            // index 0 is entry #1, index 1 is entry #2 and so on
            // this calculates the scoreboard data index from the pool index
            var extraDataIndex = poolCount - 1 - i;
            // do not break on missing data -- show error button instead
            var correspondingExtraData = extraDataIndex < __state.Scoreboard.Count
                ? __state.Scoreboard[extraDataIndex]
                : null;

            UiPatcher.FillScoreboardEntry(actualEntry, correspondingExtraData);
        }
    }
}
