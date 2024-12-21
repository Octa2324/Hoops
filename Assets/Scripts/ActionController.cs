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
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.Log("No next level found. Returning to the first level.");
            SceneManager.LoadScene(0); 
        }
    }
}
