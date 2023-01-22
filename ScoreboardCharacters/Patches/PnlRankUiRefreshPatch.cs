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
    }
}
