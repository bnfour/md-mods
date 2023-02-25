using System.Collections.Generic;
using HarmonyLib;
using Newtonsoft.Json;

using Assets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;
using System.Threading;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches
{
    /// <summary>
    /// A patch to extract additional data (character and elfin ids)
    /// from the full API response and display it as part of the scoreboard.
    /// </summary>
    [HarmonyPatch(typeof(PnlRank), "UIRefresh")]
    public class PnlRankUIRefreshPatch
    {
        private static readonly Data.AdditionalScoreboardData ScoreboardData = new Data.AdditionalScoreboardData();

        /// <summary>
        /// Prefix method to populate additional data to display from full response,
        /// which should be in "cache" dict when UIRefresh is called.
        /// </summary>
        private static void Prefix(string uid, PnlRank __instance)
        {
            ScoreboardData.Clear();

            if (__instance.m_SelfRank.ContainsKey(uid))
            {
                // this is an extremely blunt way to do this,
                // but I didn't find a way to nicely convert
                // il2cpp's JToken to a managed object
                // TODO consider better conversion
                var selfRank = JsonConvert.DeserializeObject<Data.Api.SelfRank>(__instance.m_SelfRank[uid].ToString());
                
                if (selfRank.Info != null)
                {
                    ScoreboardData.Self = new Data.AdditionalScoreboardDataEntry(selfRank.Info);
                    
                    // couldn't find a better place to update it beforehand :(
                    // TODO search for a way to permanently apply the mod UI like for the scoreboard pool
                    UiPatcher.CreateModUi(__instance.server);
                }
            }

            if (__instance.m_Ranks.ContainsKey(uid))
            {
                // same scuffed "render to a string, then use a proper library to get only relevant data" approach
                var scoreboard = JsonConvert.DeserializeObject<List<Data.Api.ScoreboardEntry>>(__instance.m_Ranks[uid].ToString());

                foreach (var entry in scoreboard)
                {
                    var data = new Data.AdditionalScoreboardDataEntry(entry.Info);
                    ScoreboardData.Scoreboard.Add(data);
                }
            }
        }

        private static void Postfix(string uid, PnlRank __instance)
        {
            // self-rank is handled separately
            if (ScoreboardData.Self != null)
            {
                UiPatcher.FillData(__instance.server, ScoreboardData.Self);
            }
            // the scoreboard itself is pooled
            // first objects seems to be the template (?), never shown on screen
            // then actual ranks to be shown, in reverse order
            // so, at most 100 objects in pool:
            //   #0 is unused,
            //   #1 is last scoreboard entry (#99 in most cases),
            //   #2 is second last scoreboard entry,
            //   ...and so on...
            //   last entry is scoreboard entry #1
            var poolCount = __instance.m_RankPool.gameObjects.Count;
            // TODO remove
            var logger = MelonLoader.Melon<ScoreboardCharactersMod>.Logger;
            for (int i = 1; i < poolCount; i++)
            {
                var actualEntry = __instance.m_RankPool.gameObjects[i];
                
                // we skip the first template entry in the for loop,
                // and the data entries are in reverse order (see notes above)
                var extraDataIndex = poolCount - 1 - i;
                // try to wait for the data to fill up to avoid index exceptions there?
                // just a wild guess
                while (extraDataIndex >= ScoreboardData.Scoreboard.Count)
                {
                    logger.Msg($"probably averted an index exception? Have {ScoreboardData.Scoreboard.Count}, want {extraDataIndex}");
                    // what if it is never updated here though?
                    Thread.Sleep(50);
                }
                var correspondingExtraData = ScoreboardData.Scoreboard[extraDataIndex];

                UiPatcher.FillData(actualEntry, correspondingExtraData);
            }
        }
    }
}
