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
        if (__instance.m_Ranks.ContainsKey(uid) && __instance.m_Ranks[uid].ToString() is string raw)
        {
            var scores = JsonConvert.DeserializeObject<List<Data.Api.ScoreboardEntry>>(raw)
                ?.Select(se => se.Info.Score) ?? [];

            Melon<RankPreviewMod>.Instance.Cache.Store(uid, scores);
        }
    }
}
