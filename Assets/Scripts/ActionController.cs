using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionController : MonoBehaviour
{
    private AudioEffects audioEffects;

    private void Start()
    {
        audioEffects = FindObjectOfType<AudioEffects>();
    }

    public void RestartLevel()
    {
        audioEffects.Select();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f; 
    }

    public void Exit()
    {
        audioEffects.Select();
        SceneManager.LoadScene("Menu");
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        audioEffects.Select();

        if (currentSceneIndex + 1 < totalScenes)
        {
            int nextLevelIndex = currentSceneIndex + 1;

            if (RuntimeDataManager.Instance.IsLevelUnlocked(nextLevelIndex))
            {
                int currentLevelStars = RuntimeDataManager.Instance.GetStarsForLevel(currentSceneIndex);
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
