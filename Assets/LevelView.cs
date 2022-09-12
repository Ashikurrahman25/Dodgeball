using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelView : MonoBehaviour
{
    public int levelIndex;
    public LevelInfo levelInfo;
    public TMP_Text levelName;
    public GameObject selectedTag;
    public Image levelPreview;

    public void SetUp(LevelInfo levelInfo, int levelIndex)
    {
        this.levelInfo = levelInfo;
        this.levelIndex = levelIndex;

        levelName.text = levelInfo.levelName;

        if (levelInfo.levelSprite != null)
            levelPreview.sprite = levelInfo.levelSprite;

        UpdateBuyUI();

    }


    public void UpdateBuyUI()
    {
        if (levelInfo.isSelected)
            selectedTag.SetActive(true);

        else
            selectedTag.SetActive(false);
    }


    public void OnBuyButton()
    {
        LevelManager manager = FindObjectOfType<LevelManager>();

        levelInfo.isSelected = true;
        manager.levels[levelIndex].isSelected = true;
        PlayerPrefs.SetInt("SelectedLevel", levelIndex);
        PlayerPrefs.SetString("Level", levelInfo.sceneName);
        manager.UpdateUI();
    }
}
