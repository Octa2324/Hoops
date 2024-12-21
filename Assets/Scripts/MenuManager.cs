using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public List<Sprite> backgroundSprites;
    public List<Sprite> ballSprites;
    public Image backgroundImage; 
    public Image ballImage;
    private int selectedBackgroundIndex = 0;
    private int selectedBallIndex = 0;

    void Start()
    {
        selectedBackgroundIndex = PlayerPrefs.GetInt("SelectedBackground", 0);
        SetBackground(selectedBackgroundIndex);

        selectedBallIndex = PlayerPrefs.GetInt("SelectedBall", 0);
        SetBall(selectedBallIndex);
    }

    public void NextBackground()
    {
        selectedBackgroundIndex = (selectedBackgroundIndex + 1) % backgroundSprites.Count;
        SetBackground(selectedBackgroundIndex);
    }

    public void PreviousBackground()
    {
        selectedBackgroundIndex = (selectedBackgroundIndex - 1 + backgroundSprites.Count) % backgroundSprites.Count;
        SetBackground(selectedBackgroundIndex);
    }

    public void NextBall()
    {
        selectedBallIndex = (selectedBallIndex + 1) % ballSprites.Count;
        SetBall(selectedBallIndex);
    }

    public void PreviousBall()
    {
        selectedBallIndex = (selectedBallIndex - 1 + ballSprites.Count) % ballSprites.Count;
        SetBall(selectedBallIndex);
    }

    private void SetBall(int index)
    {
        ballImage.sprite = ballSprites[index];
        PlayerPrefs.SetInt("SelectedBall", selectedBallIndex);
        PlayerPrefs.Save();
    }

    private void SetBackground(int index)
    {
        backgroundImage.sprite = backgroundSprites[index]; 
        PlayerPrefs.SetInt("SelectedBackground", selectedBackgroundIndex); 
        PlayerPrefs.Save();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
}
