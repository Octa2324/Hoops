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

    public List<Sprite> trajectoryColorImages;
    public List<Color> trajectoryColors;
    public Image trajectoryColorPreview; 
    private int selectedTrajectoryColorIndex = 0;

    private AudioEffects audioEffects;


    void Start()
    {
        selectedBackgroundIndex = RuntimeDataManager.Instance.SelectedBackground;
        SetBackground(selectedBackgroundIndex);

        selectedBallIndex = RuntimeDataManager.Instance.SelectedBall;
        SetBall(selectedBallIndex);

        selectedTrajectoryColorIndex = PlayerPrefs.GetInt("SelectedTrajectoryIndex", 0);
        SetTrajectoryColor(selectedTrajectoryColorIndex);

        audioEffects = FindObjectOfType<AudioEffects>();
    }

    public void NextBackground()
    {
        audioEffects.Select();
        selectedBackgroundIndex = (selectedBackgroundIndex + 1) % backgroundSprites.Count;
        SetBackground(selectedBackgroundIndex);
    }

    public void PreviousBackground()
    {

        audioEffects.Select();
        selectedBackgroundIndex = (selectedBackgroundIndex - 1 + backgroundSprites.Count) % backgroundSprites.Count;
        SetBackground(selectedBackgroundIndex);
    }

    public void NextBall()
    {
        audioEffects.Select();
        selectedBallIndex = (selectedBallIndex + 1) % ballSprites.Count;
        SetBall(selectedBallIndex);
    }

    public void PreviousBall()
    {
        audioEffects.Select();
        selectedBallIndex = (selectedBallIndex - 1 + ballSprites.Count) % ballSprites.Count;
        SetBall(selectedBallIndex);
    }

    private void SetBall(int index)
    {
            ballImage.sprite = ballSprites[index];
            RuntimeDataManager.Instance.SelectedBall = index;
    }

    private void SetBackground(int index)
    {
            backgroundImage.sprite = backgroundSprites[index];
            RuntimeDataManager.Instance.SelectedBackground = index;
    }

    public void NextTrajectoryColor()
    {
        audioEffects.Select();
        selectedTrajectoryColorIndex = (selectedTrajectoryColorIndex + 1) % trajectoryColors.Count;
        SetTrajectoryColor(selectedTrajectoryColorIndex);
    }

    public void PreviousTrajectoryColor()
    {
        audioEffects.Select();
        selectedTrajectoryColorIndex = (selectedTrajectoryColorIndex - 1 + trajectoryColors.Count) % trajectoryColors.Count;
        SetTrajectoryColor(selectedTrajectoryColorIndex);
    }

    private void SetTrajectoryColor(int index)
    {
        trajectoryColorPreview.sprite = trajectoryColorImages[index];

        Color colorToSave = trajectoryColors[index];
        PlayerPrefs.SetFloat("TrajectoryColorR", colorToSave.r);
        PlayerPrefs.SetFloat("TrajectoryColorG", colorToSave.g);
        PlayerPrefs.SetFloat("TrajectoryColorB", colorToSave.b);
        PlayerPrefs.SetInt("SelectedTrajectoryIndex", index);
        PlayerPrefs.Save();

        Debug.Log($"Saved trajectory color: {colorToSave}");
    }

    public void PlayGame()
    {
        audioEffects.Select();
        SceneManager.LoadScene("Level 1");
    }


}
