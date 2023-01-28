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
                // TODO proper control and proper setup
                var toCopy = rankCell.transform.FindChild(BaseTextfieldId);

                var duplicate = GameObject.Instantiate(toCopy);
                duplicate.name = NewTextfieldId;

                duplicate.transform.SetParent(rankCell.transform);
                // TODO set position in a way it survives instantiation from the pool
                // instead of positioning on UI refresh

                // for whatever reason, scale for self rank cell is set to 100 100 100 by default
                var scaleFix = new Vector3(1, 1, 1);
                duplicate.transform.localScale = scaleFix;

                duplicate.gameObject.layer = rankCell.layer;
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

                // not a fan of moving the component at the last moment, but no idea how to set position
                // for a pool template and not have it reset upon instantiating
                // hardcoded for 1080 screen (?)
                extraField.transform.localPosition = new Vector3(515, -20, 0);
            }
        }
    }
}
