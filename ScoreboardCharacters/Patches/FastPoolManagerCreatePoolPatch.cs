using System.Runtime.InteropServices;

using HarmonyLib;
using UnityEngine;

using Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches
{
    [HarmonyPatch(typeof(FastPoolManager), nameof(FastPoolManager.CreatePool), new[] {typeof(GameObject), typeof(bool), typeof(int), typeof(int), typeof(Transform)})]
    public class FastPoolManagerCreatePoolPatch
    {
        // all these unused arguments are required for the method to pass as a patch for the method
        #pragma warning disable IDE0060

        // hook the creation of pool of reused objects for scoreboard entries
        // just before it's populated and modify the template to our needs,
        // so every instance from the pool has our extra component
        private static void Prefix(GameObject prefab, bool warmOnLoad, int preloadCount, int capacity, [Optional] Transform newRootTransform)
        {
            if (prefab.name == "RankCell_4-3")
            {
                UiPatcher.CreateModUi(prefab);
            }
        }

        #pragma warning restore IDE0060
    }
}
