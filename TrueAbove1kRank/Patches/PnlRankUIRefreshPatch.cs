using HarmonyLib;
using Newtonsoft.Json;

using Assets.Scripts.UI.Panels;

namespace Bnfour.MuseDashMods.TrueAbove1kRank.Patches
{
    [HarmonyPatch(typeof(PnlRank), nameof(PnlRank.UIRefresh))]
    public class PnlRankUIRefreshPatch
    {
        private static void Postfix(string uid, PnlRank __instance)
        {
            if (__instance.m_SelfRank.ContainsKey(uid)
                && __instance.txtServerRank.IsActive()
                && __instance.txtServerRank.text.Contains("+"))
            {
                var selfRank = JsonConvert.DeserializeObject<Data.Api.SelfRank>(__instance.m_SelfRank[uid].ToString());
                __instance.txtServerRank.text = selfRank.DisplayRank.ToString();
            }
        }
    }
}
