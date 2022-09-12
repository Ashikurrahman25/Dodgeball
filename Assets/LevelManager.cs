using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int selectedLevel;
    public LevelView levelView;
    public TMP_Text selectedLevelText;
    public Transform levelHolder;
    public List<LevelView> instantiatedPlayers = new List<LevelView>();
    public LevelInfo[] levels;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("SelectedLevel"))
            PlayerPrefs.SetInt("SelectedLevel", 0);

        selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 0);
        selectedLevelText.text = levels[selectedLevel].levelName;

        for (int i = 0; i < levels.Length; i++)
        {
            if (PlayerPrefs.GetInt("SelectedLevel", 0) == i)
                levels[i].isSelected = true;

            InstantiatePlayerUI(levels[i], i);
        }
    }


    void InstantiatePlayerUI(LevelInfo levelInfo, int index)
    {
        LevelView player = Instantiate(levelView, levelHolder);
        player.SetUp(levelInfo, index);
        instantiatedPlayers.Add(player);
    }
  

    public void UpdateUI()
    {
        Debug.LogWarning(PlayerPrefs.GetInt("SelectedLevel", 0));

        for (int i = 0; i < levels.Length; i++)
        {
            if (PlayerPrefs.GetInt("SelectedLevel", 0) == i)
            {
                selectedLevel = i;
                levels[i].isSelected = true;
            }
            else
                levels[i].isSelected = false;
        }

        for (int i = 0; i < instantiatedPlayers.Count; i++)
        {
            instantiatedPlayers[i].levelInfo = levels[i];
            instantiatedPlayers[i].UpdateBuyUI();
        }

        selectedLevelText.text = levels[selectedLevel].levelName;
    }

}

[System.Serializable]
public class LevelInfo
{
    public string levelName;
    public string sceneName;
    public Sprite levelSprite;
    public bool isSelected;
}
