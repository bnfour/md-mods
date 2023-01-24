using System.Collections.Generic;
using HarmonyLib;
using MelonLoader;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine;

using Assets.Scripts.UI.Panels;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

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

                    // couldn't find a better place to update it beforehand :(
                    // add extra components to self rank cell
                    UiPatcher.CreateModUi(__instance.server);
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
                var selfRankCell = __instance.server.gameObject;
                var extraField = selfRankCell.transform.FindChild("BnExtraTextField");
                if (extraField != null)
                {
                    var textComponent = extraField.GetComponent<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = scoreboardData.Self.ToString();
                    }
                }
            }
            // the scoreboard itself is pooled
            // first objects seems to be the template (?), never shown on screen
            // then actual ranks to be shown, in reverse order
            // so, 100 objects in pool:
            //   #0 is unused,
            //   #1 is scoreboard #99,
            //   #2 is scoreboard #98,
            //   and so on
            for (int i = 1; i < __instance.m_RankPool.gameObjects.Count; i++)
            {
                var actualEntry = __instance.m_RankPool.gameObjects[i]; //.GetComponent<RankCell>(); ??
                // these can be mapped to RankCell, but we will interact with components not present in it,
                // but added in runtime

                // we skip the first template entry in the for loop,
                // and also entries seem to be in reverse order (see notes above)
                // so index is adjusted
                var correspondingExtraData = scoreboardData.Scoreboard[99 - i];

                var extraField = actualEntry.transform.FindChild("BnExtraTextField");
                if (extraField != null)
                {
                    var textComponent = extraField.GetComponent<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = correspondingExtraData.ToString();
                    }
                }
            }
        }
    }
}
