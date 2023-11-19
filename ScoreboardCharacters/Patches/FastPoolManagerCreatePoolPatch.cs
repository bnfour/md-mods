using HarmonyLib;
using UnityEngine;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches
{
    [HarmonyPatch(typeof(FastPoolManager), nameof(FastPoolManager.CreatePool), new[] {typeof(GameObject), typeof(bool), typeof(int), typeof(int), typeof(Transform)})]
    public class FastPoolManagerCreatePoolPatch
    {
        // hook the creation of pool of reused objects for scoreboard entries
        // just before it's populated and modify the template to our needs,
        // so every instance from the pool has our extra component
        private static void Prefix(GameObject prefab)
        {
            if (prefab.name == "RankCell_4-3")
            {
                UiPatcher.CreateModUi(prefab);
            }
        }
    }
}
