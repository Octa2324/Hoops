using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionController : MonoBehaviour
{
    public void RestartLevel()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f; 
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex + 1 < totalScenes)
        {
            int nextLevelIndex = currentSceneIndex + 1;
            int isUnlocked = PlayerPrefs.GetInt("LevelUnlocked_" + nextLevelIndex, 0);
            if (isUnlocked == 1)
            {
                int currentLevelStars = PlayerPrefs.GetInt("Stars_Level_" + currentSceneIndex, 0);
                if (currentLevelStars >= 3)
                {
                    SceneManager.LoadScene(nextLevelIndex);
                }
                else
                {
                    Debug.Log("Not enough stars to proceed. Redirecting to menu.");
                    Exit(); 
                }
            }
            else
            {
                Debug.Log("Next level is locked. Complete previous levels to unlock it.");
                Exit(); 
            }
        }
        else
        {
            Debug.Log("No next level found. Returning to the first level.");
            SceneManager.LoadScene(0); 
        }
    }

}
