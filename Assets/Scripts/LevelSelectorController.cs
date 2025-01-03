using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorController : MonoBehaviour
{
    [System.Serializable]
    public class LevelButton
    {
        public Button button;           
        public Image starImage;         
        public List<Sprite> starSprites; 
    }

    public List<LevelButton> levelButtons; 

    private void Start()
    {
        UpdateLevelStars();

        UpdateLevelAccessibility();
    }

    private void UpdateLevelStars()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            int stars = PlayerPrefs.GetInt("Stars_Level_" + (i + 1), 0); 
            if (levelButtons[i].starImage != null && stars < levelButtons[i].starSprites.Count)
            {
                levelButtons[i].starImage.sprite = levelButtons[i].starSprites[stars];
            }
        }
    }


    private void UpdateLevelAccessibility()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            int starsRequired = 3; 
            if (i == 0)
            {
                levelButtons[i].button.interactable = true;
            }
            else
            {
                int previousLevelStars = PlayerPrefs.GetInt("Stars_Level_" + i, 0);
                levelButtons[i].button.interactable = previousLevelStars >= starsRequired;
            }
        }
    }
}
