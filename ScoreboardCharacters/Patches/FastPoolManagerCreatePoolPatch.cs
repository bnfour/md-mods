using System;
using System.Runtime.InteropServices;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Patches
{
    [HarmonyPatch(typeof(FastPoolManager), "CreatePool", new[] {typeof(GameObject), typeof(bool), typeof(int), typeof(int), typeof(Transform)})]
    public class FastPoolManagerCreatePoolPatch
    {
        // hook the creation of pool of reused objects for scoreboard entries
        // just before it's populated and modify the template to our needs
        private static void Prefix(GameObject prefab, bool warmOnLoad, int preloadCount, int capacity, [Optional] Transform newRootTransform)
        {
            if (prefab.name == "RankCell_4-3")
            {
                // TODO this should actually be a button, but for now we copy existing text label
                // just to see how things work
                var alreadyAdded = prefab.transform.FindChild("BnExtraTextField");
                if (alreadyAdded == null)
                {
                    // copy existing thing for now, so most setup is already done
                    var toCopy = prefab.transform.FindChild("TxtIdValueS");
                    var duplicate = GameObject.Instantiate(toCopy);
                    duplicate.name = "BnExtraTextField";
                    duplicate.GetComponent<RectTransform>().SetParent(prefab.transform);
                    // TODO figure out how to deal with positioning when we patch a template
                    // unlike self rank where we patch created object because i could not find its template (yet?)
                    duplicate.gameObject.layer = prefab.layer;
                    var textComponent = duplicate.GetComponent<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = "test message please ignore";
                    }
                    duplicate.transform.SetParent(prefab.transform);
                }
            }
        }
    }
}
