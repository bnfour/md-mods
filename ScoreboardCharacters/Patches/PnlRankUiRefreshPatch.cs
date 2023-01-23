using System;
using HarmonyLib;
using MelonLoader;
using Newtonsoft.Json;

using Assets.Scripts.UI.Panels;
using System.Collections.Generic;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches
{
    /// <summary>
    /// A patch to extract additional data (character and elfin ids)
    /// from the full API response and store it for later display.
    /// </summary>
    [HarmonyPatch(typeof(PnlRank), "UIRefresh")]
    public class PnlRankUiRefreshPatch
    {
        /// <summary>
        /// Prefix method to populate additional data to display from full response,
        /// which should be in "cache" dict when UIRefresh is called.
        /// </summary>
        private static void Prefix(string uid, PnlRank __instance)
        {
            var scoreboardData = Melon<ScoreboardCharactersMod>.Instance.ScoreboardData;
            scoreboardData.Clear();

            if (__instance.m_SelfRank.ContainsKey(uid))
            {
                // this is an extremely blunt way to do this,
                // but I didn't find a way to nicely convert
                // il2cpp's JToken to a managed object
                // TODO consider better conversion
                var selfRank = JsonConvert.DeserializeObject<Data.Api.SelfRank>(__instance.m_SelfRank[uid].ToString());
                
                if (selfRank.Detail != null)
                {
                    scoreboardData.Self = new Data.AdditionalScoreboardDataEntry
                    {
                        CharacterId = selfRank.Detail.CharacterId,
                        ElfinId = selfRank.Detail.ElfinId
                    };
                }
            }

            if (__instance.m_Ranks.ContainsKey(uid))
            {
                var scoreboard = JsonConvert.DeserializeObject<List<Data.Api.ScoreboardEntry>>(__instance.m_Ranks[uid].ToString());

                foreach (var entry in scoreboard)
                {
                    var storedData = new Data.AdditionalScoreboardDataEntry
                    {
                        CharacterId = entry.Detail.CharacterId,
                        ElfinId = entry.Detail.ElfinId
                    };
                    scoreboardData.Scoreboard.Add(storedData);
                }
            }
        }

        private static void Postfix(string uid, PnlRank __instance)
        {
            // if we're really setting this here, we won't need to include scoreboardData in the Mod itself,
            // it will be only ever used in this class
            var scoreboardData = Melon<ScoreboardCharactersMod>.Instance.ScoreboardData;

            // self-rank is handled separately
            if (scoreboardData.Self != null)
            {
                // this is the "PlayerRankCell_4-3" used for self rank
                var selfRankCell = __instance.server;
                // TODO patch somewhere (constructor?) to add fields to store extra data,
                // set values from ScoreboardData.Self
            }
            // the scoreboard itself is pooled
            // first objects seems to be the template (?), never shown on screen
            for (int i = 1; i < __instance.m_RankPool.gameObjects.Count; i++)
            {
                var actualEntry = __instance.m_RankPool.gameObjects[i]; //.GetComponent<RankCell>(); ??
                // these can be mapped to RankCell, but we will interact with components not present in it,
                // but added in runtime

                // we skip the first template entry in the for loop, so index is adjusted for extra data
                var correspondingExtraData = scoreboardData.Scoreboard[i - 1];

                // TODO patch somewhere to add fields to store extra data,
                // set values from correspondingExtraData
            }
        }
    }
}
