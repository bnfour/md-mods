using System.Collections.Generic;
using HarmonyLib;
using Newtonsoft.Json;

using Il2CppAssets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;
using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

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
        __state = new AdditionalScoreboardData();

        if (__instance.m_SelfRank.ContainsKey(uid))
        {
            // this is an extremely blunt way to do this,
            // but I didn't find a way to nicely convert
            // il2cpp's JToken to a managed object
            // TODO consider better conversion
            var selfRank = JsonConvert.DeserializeObject<Data.Api.SelfRank>(__instance.m_SelfRank[uid].ToString());
            
            if (selfRank != null && selfRank.Info != null)
            {
                __state.Self = new AdditionalScoreboardDataEntry(selfRank.Info);
                
                // couldn't find a better place to update it beforehand :(
                // TODO search for a way to permanently apply the mod UI like for the scoreboard pool
                UiPatcher.CreateModUi(__instance.server);
            }
        }

        if (__instance.m_Ranks.ContainsKey(uid))
        {
            // same scuffed "render to a string, then use a proper library to get only relevant data" approach
            var scoreboard = JsonConvert.DeserializeObject<List<Data.Api.ScoreboardEntry>>(__instance.m_Ranks[uid].ToString());

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
        if (__state.Self != null)
        {
            UiPatcher.FillData(__instance.server, __state.Self, __instance);
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
            // check for missing data and let the user know
            if (extraDataIndex >= __state.Scoreboard.Count)
            {
                // a few words on the issue (tracked as #5):
                // so far, it very rarely happens randomly (incomplete server response?)
                // or sometimes when *quickly* switching difficulties (data reset before this loop is completed?)
                // it's not that common (didn't have this in months) or breaking (a refresh fixes it), so just a warning for now
                var logger = MelonLoader.Melon<ScoreboardCharactersMod>.Logger;
                logger.Warning($"Unable to fill the entire scoreboard. Try refreshing if you see an incomplete scoreboard.");
                break;
            }
            var correspondingExtraData = __state.Scoreboard[extraDataIndex];

            UiPatcher.FillData(actualEntry, correspondingExtraData, __instance);
        }
    }
}
