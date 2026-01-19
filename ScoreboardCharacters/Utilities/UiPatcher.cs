using MelonLoader;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;
using Il2CppPeroPeroGames.GlobalDefines;
using Il2CppInterop.Runtime;

using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities;

/// <summary>
/// Creates extra UI at runtime, sets click handlers.
/// </summary>
// static to be called in patches
public static class UiPatcher
{
    private const string ScoreboardEntryComponentName = "BnScoreboardCharactersExtra";
    private const string CustomLevelConfigComponentName = "BnScoreboardCharactersLevelConfig";

    private const string CustomLevelConfigPath = "UI/Standerd/PnlPreparation/RightRoot/Top/RootLevelConfigShow/" + CustomLevelConfigComponentName;

    private const float LevelConfigInnerScale = 1.25f;
    private const int ToggleLineExtraHeight = 6;
    // see #34
    private static readonly Vector3 JapaneseLocalePositionCorrection = new(54f, 0, 0);

    public static void CreateModUiForScoreboardEntry(GameObject rankCell)
    {
        var addedComponent = rankCell.transform.FindChild(ScoreboardEntryComponentName);
        if (addedComponent == null)
        {
            var buttonObj = DefaultControls.CreateButton(new DefaultControls.Resources());
            buttonObj.name = ScoreboardEntryComponentName;

            // default button also contains a text component, we're not using it
            var text = buttonObj.GetComponentInChildren<Text>();
            text.transform.parent = null;

            buttonObj.transform.SetParent(rankCell.transform);
            // for whatever reason, scale for self rank cell is set to 100 100 100 by default
            buttonObj.transform.localScale = new Vector3(1, 1, 1);

            var rect = buttonObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(80, 40);
            rect.anchoredPosition3D = new Vector3(200, 0, 0) + PixelPerfectishOffsetCorrection(rankCell);
        }
    }

    public static void FillScoreboardEntry(GameObject rankCell, AdditionalScoreboardDataEntry dataEntry)
    {
        var extraField = rankCell.transform.FindChild(ScoreboardEntryComponentName);
        if (extraField != null)
        {
            var buttonComponent = extraField.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.RemoveAllListeners();
                buttonComponent.onClick.AddListener((UnityAction)new System.Action(() =>
                {
                    var switcher = Melon<ScoreboardCharactersMod>.Instance.CharacterSwitcher;
                    switcher.Switch(dataEntry.Character, dataEntry.Elfin);
                }));

                var imageComponent = buttonComponent.image;
                if (imageComponent != null)
                {
                    var provider = Melon<ScoreboardCharactersMod>.Instance.ButtonImageProvider;
                    imageComponent.sprite = provider.GetSprite(dataEntry.Character, dataEntry.Elfin);
                }
            }
        }
    }

    public static void ModifyLevelConfigUI(PnlPreparation panel)
    {
        var levelConfigUIGroup = panel.transform.Find("RightRoot/Top");
        if (levelConfigUIGroup != null)
        {
            // move entire group
            levelConfigUIGroup.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(-1180, -214, 0);
            // move a bit more if the game is running in Japanese, see #34
            if (GlobalDataBase.s_DbUi.curLanguageIndex == Language.japanese)
            {
                levelConfigUIGroup.GetComponent<RectTransform>().anchoredPosition3D += JapaneseLocalePositionCorrection;
            }

            // add custom image as a component in a custom object,
            // set to neutral positioning/scaling -- everything is done in the image component
            var holderObject = new GameObject(CustomLevelConfigComponentName, Il2CppType.Of<Image>());
            holderObject.transform.parent = levelConfigUIGroup.Find("RootLevelConfigShow").transform;
            holderObject.transform.position = new(0, 0, 0);
            holderObject.transform.localScale = new(1, 1, 1);
            // move below the vanilla BtnXXXLevelConfig components so it does not block mouse clicks
            holderObject.transform.SetSiblingIndex(holderObject.transform.GetSiblingIndex() - 2);

            var currentConfigImage = holderObject.GetComponent<Image>();
            // 1x scale results in the image being 64x32 on 1080
            currentConfigImage.rectTransform.localScale = new(LevelConfigInnerScale, LevelConfigInnerScale, 1);
            currentConfigImage.rectTransform.sizeDelta = new(80, 40);
            // just to the right of config lock button, taking scale and pixel perfectish offsets into account:
            // 43 is amount of _screen_ pixels (in 1080) to move,
            // offsets are to clamp position to whole pixels
            currentConfigImage.rectTransform.anchoredPosition3D = new(-43 * LevelConfigInnerScale + 0.27f, 0.37f, 0);
            currentConfigImage.color = Color.white;

            // the character/elfin text is hereby banished to the shadow realm until further notice
            var scrollTextTransform = levelConfigUIGroup.Find("RootLevelConfigShow/ImgArtistMask")?.GetComponent<RectTransform>();
            scrollTextTransform?.anchoredPosition3D = new(99_999, 99_999, 0);

            // make component narrower
            // includes replacing the bg image, moving E button hint,
            // moving the entire random toggle, and adjusting the hitbox

            // background image is a bit tricky because its size and position affects child components
            // so we just hide the original one to not disturb the positioning,
            // and instead show a resized clone that does not affect anything
            var originalImage = levelConfigUIGroup.Find("RootLevelConfigShow")?.GetComponent<Image>();
            if (originalImage != null)
            {
                // make the clone a child of the original
                var clonedImage = GameObject.Instantiate(originalImage, originalImage.transform);
                clonedImage.name = "BnNarrowBackground";
                // remove all the cloned child components, we only want the image itself
                while (clonedImage.transform.childCount > 0)
                {
                    GameObject.DestroyImmediate(clonedImage.transform.GetChild(0).gameObject);
                }
                // set as the first sibling so it's rendered first as a background for everything else
                clonedImage.rectTransform.SetAsFirstSibling();
                clonedImage.rectTransform.sizeDelta = new(150 * LevelConfigInnerScale, clonedImage.rectTransform.sizeDelta.y + ToggleLineExtraHeight * LevelConfigInnerScale);
                clonedImage.rectTransform.anchoredPosition3D = new(-42 * LevelConfigInnerScale, 0, 0);
                // hide the original image
                originalImage.color = Color.clear;
            }
            // hitbox that opens the character/elfin select popup:
            // adjust to match the visible UI
            if (levelConfigUIGroup.Find("RootLevelConfigShow/BtnOpenPnllevelConfig")
                ?.GetComponent<Image>()?.rectTransform is RectTransform popupHitbox)
            {
                // 8 is pure move to the right; -125/2 is accounting to the width change
                // TODO see if anchoring could help with this
                popupHitbox.anchoredPosition3D += LevelConfigInnerScale * new Vector3(8 - 125f / 2, 0, 0);
                popupHitbox.sizeDelta += LevelConfigInnerScale * new Vector2(-125, 0);
            }
            // the small "E" button hint
            var eTransform = levelConfigUIGroup.Find("RootLevelConfigShow/BtnOpenPnllevelConfig/ImgRandomPCtipBg (2)")?.GetComponent<RectTransform>();
            eTransform?.anchoredPosition3D += new Vector3(-50 * LevelConfigInnerScale, 0, 0);
            // move the hitbox for the "Lock level config" button as well
            // still no idea what it does exactly ¯\_(ツ)_/¯
            if (levelConfigUIGroup.Find("RootLevelConfigShow/BtnLockLevelConfig")
                ?.GetComponent<Image>()?.rectTransform is RectTransform lockHitbox)
            {
                lockHitbox.anchoredPosition3D += LevelConfigInnerScale * new Vector3(8, 0, 0);
            }
            // move the "Q" button hint as well (its original position is mangled somewhere)
            if (levelConfigUIGroup.Find("RootLevelConfigShow/BtnOpenPnllevelConfig/ImgLevelConfigPCTipBack")
                ?.GetComponent<Image>()?.rectTransform is RectTransform qTransform)
            {
                qTransform.anchoredPosition3D += LevelConfigInnerScale * new Vector3(55, 0, 0);
            }

            // random mode toggle: move and change thickness to match the other one,
            var randomToggleTransform = levelConfigUIGroup.Find("ImgRandomBg")?.GetComponent<RectTransform>();
            randomToggleTransform?.anchoredPosition3D += new Vector3(-116 * LevelConfigInnerScale, 0, 0);
            var randomToggleBg = levelConfigUIGroup.Find("ImgRandomBg")?.GetComponent<Image>();
            randomToggleBg?.rectTransform.sizeDelta += new Vector2(0, ToggleLineExtraHeight * LevelConfigInnerScale);
            // random mode toggle: move the key hint up to not hang below the panel and to math the other one
            var buttonTransform = levelConfigUIGroup.Find("ImgRandomBg/ImgRandomFlagBg/KeyTip")?.GetComponent<RectTransform>();
            buttonTransform?.anchoredPosition3D += new Vector3(0, 2, 0);
            // random mode toggle: resize and move the image inside the button component 
            // used to react to mouse clicks; to match the updated positioning&sizing
            if (levelConfigUIGroup.Find("ImgRandomBg/BtnRandomReset")
                ?.GetComponent<Image>()?.rectTransform is RectTransform randomToggleHitbox)
            {
                randomToggleHitbox.anchoredPosition3D += LevelConfigInnerScale * new Vector3(10, 3, 0);
                randomToggleHitbox.sizeDelta += LevelConfigInnerScale * new Vector2(4, 2 * ToggleLineExtraHeight);
            }

            // update the sprite on creation so it shows the current config on panel open
            UpdateLevelConfigUI();
        }
    }

    public static void FixRankPanelHeight(PnlRank instance)
    {
        var bg = instance.transform.Find("Mask/ImgTittleBaseP/ImgRankTips");
        var rt = bg?.GetComponent<RectTransform>();
        rt?.sizeDelta += new Vector2(0, 1f);
        rt?.anchoredPosition3D += new Vector3(0, -0.5f, 0);
    }

    public static void UpdateLevelConfigUI()
    {
        var image = GameObject.Find(CustomLevelConfigPath)?.GetComponent<Image>();
        image?.sprite = Melon<ScoreboardCharactersMod>.Instance.ButtonImageProvider.GetSprite
            (
                (Character)DataHelper.selectedRoleIndex,
                (Elfin)DataHelper.selectedElfinIndex
            );
    }

    public static void ApplyLocaleSpecificOffset(int multiplier)
    {
        // positive multiplier is "to japanese",
        // negative is reverse
        // TODO restrict to ±1 specifically?
        GameObject.Find("UI/Standerd/PnlPreparation/RightRoot/Top")
            ?.GetComponent<RectTransform>()
            ?.anchoredPosition3D += multiplier * JapaneseLocalePositionCorrection;
    }

    // empirically found offsets to snap the sprites to whole-pixel grid close enough to prevent noticeable smudging
    private static Vector3 PixelPerfectishOffsetCorrection(GameObject _)
    {
        // currently the same value for scoreboard entries and self rank;
        // GameObject argument is kept as a discard in case it'll be different again in the future
        // (see history to learn how to discern by name)
        return new(0.5f, -0.08f, 0);
    }
}
