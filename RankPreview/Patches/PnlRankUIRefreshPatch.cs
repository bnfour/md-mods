using System;
using System.Collections.Generic;
using System.Linq;

using HarmonyLib;
using MelonLoader;
using Newtonsoft.Json;

using Il2CppAssets.Scripts.UI.Panels;

namespace Bnfour.MuseDashMods.RankPreview.Patches;

[HarmonyPatch(typeof(PnlRank), nameof(PnlRank.UIRefresh))]
public class PnlRankUIRefreshPatch
{
    internal static void Postfix(string uid, PnlRank __instance)
    {
        var logger = Melon<RankPreviewMod>.Logger;

        logger.Msg($"ambatu store data for {uid}");

        if (__instance.m_Ranks.ContainsKey(uid) && __instance.m_Ranks[uid].ToString() is string raw)
        {
            // logger.Msg($"=== RAW DATA:\n{__instance.m_Ranks[uid].ToString()}\n=== END OF DATA");
            var scores = JsonConvert.DeserializeObject<List<Data.Api.ScoreboardEntry>>(raw)
                ?.Select(se => se.Info.Score).ToArray() ?? Array.Empty<int>();

            logger.Msg($"got score data: [{string.Join(", ", scores)}]\nI'M STORING");
            Melon<RankPreviewMod>.Instance.Cache[uid] = scores;
        }
        else
        {
            logger.Msg($"no scoreboard for {uid}");
        }
    }
}
