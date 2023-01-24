using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities
{
    /// <summary>
    /// A class to encapsulate operations with added UI elements.
    /// </summary>
    // static to be called in patches
    public static class UiPatcher
    {
        private const string NewTextfieldId = "BnExtraTextField";
        private const string BaseTextfieldId = "TxtIdValueS";

        public static void CreateModUi(GameObject rankCell)
        {
            var addedComponent = rankCell.transform.FindChild(NewTextfieldId);
            if (addedComponent == null)
            {
                // just copy existing thing for now, so most setup is already done
                // TODO proper control setup
                var toCopy = rankCell.transform.FindChild(BaseTextfieldId);

                var duplicate = GameObject.Instantiate(toCopy);
                duplicate.name = NewTextfieldId;
                duplicate.GetComponent<RectTransform>().SetParent(rankCell.transform);

                var transform = duplicate.GetComponent<RectTransform>();
                // for whatever reason, scale for self rank cell is set to 100 100 100 by default
                var scaleFix = new Vector3(1, 1, 1);
                // position is hardcoded for 1080 resolution for now
                // for whatever reason, only works for self rank cell
                var localPositionFix = new Vector3(550, -20, 0);
                transform.set_localScale_Injected(ref scaleFix);
                transform.set_localPosition_Injected(ref localPositionFix);

                duplicate.gameObject.layer = rankCell.layer;
                duplicate.transform.SetParent(rankCell.transform);
            }
        }

        public static void FillData(GameObject rankCell, Data.AdditionalScoreboardDataEntry dataEntry)
        {
            var extraField = rankCell.transform.FindChild(NewTextfieldId);
            if (extraField != null)
            {
                var textComponent = extraField.GetComponent<Text>();
                if (textComponent != null)
                {
                    textComponent.text = dataEntry.ToString();
                }
            }
        }
    }
}
