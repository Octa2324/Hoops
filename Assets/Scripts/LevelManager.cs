using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public SpriteRenderer gameBackground; 
    public List<Sprite> gameBackgroundSprites;

    public SpriteRenderer gameBall;
    public List<Sprite> gameBallSprites;


    Camera cam;

    public BallController ball;
    private BallController defaultBall;
    public Trajectory trajectory;
    [SerializeField] float PushForce = 4f;

    bool isDragging;

    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector2 force;
    float distance;


    private int totalCoins = 3;
    private int currentCoins = 0;


    public GameObject pauseScreen;
    public GameObject nextLevelCanvas; 
    public GameObject gameOverCanvas;
    public GameObject finalCanvas;
    private CanvasGroup nextLevelCanvasGroup;
    private CanvasGroup gameOverCanvasGroup;
    private CanvasGroup finalCanvasGroup;

    public GameObject pauseCanvas;

    public List<Sprite> starImages; 
    public Image stars;

    Scoring scoring;

    private bool isInputLocked = false;

    private bool hasScored = false;

    private bool isGameOver = false;

    private bool hasShot = false;

    private AudioEffects audioEffects;

    public List<Color> trajectoryColors; 
    private Color selectedTrajectoryColor; 


    private void Start()
    {   
        scoring = FindAnyObjectByType<Scoring>();

        cam = Camera.main;

        if (nextLevelCanvas != null)
            nextLevelCanvasGroup = nextLevelCanvas.GetComponent<CanvasGroup>();
        if (gameOverCanvas != null)
            gameOverCanvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();
        if (finalCanvas != null)
            finalCanvasGroup = finalCanvas.GetComponent<CanvasGroup>();

        int selectedBackgroundIndex = PlayerPrefs.GetInt("SelectedBackground", 0);
        SetGameBackground(selectedBackgroundIndex);

        int selectedBallIndex = PlayerPrefs.GetInt("SelectedBall", 0);
        SetGameBall(selectedBallIndex);


        scoring.OnScored += HandleScore;

        audioEffects = FindObjectOfType<AudioEffects>();

        selectedTrajectoryColor = GetSavedTrajectoryColor();
        SetTrajectoryColor(selectedTrajectoryColor);
    }

    private Color GetSavedTrajectoryColor()
    {
        float r = PlayerPrefs.GetFloat("TrajectoryColorR", 0.118f); 
        float g = PlayerPrefs.GetFloat("TrajectoryColorG", 0.537f);
        float b = PlayerPrefs.GetFloat("TrajectoryColorB", 0.741f);
        return new Color(r, g, b);
    }

    private void SetTrajectoryColor(Color color)
    {
        if (trajectory != null)
        {
            trajectory.ApplyTrajectoryColor(color);
        }
    }

    private void SetGameBackground(int index)
    {
        if (index >= 0 && index < gameBackgroundSprites.Count)
        {
            gameBackground.sprite = gameBackgroundSprites[index];
        }
    }

    private void SetGameBall(int index)
    {
        if(index >= 0 && index < gameBallSprites.Count)
        {
            gameBall.sprite = gameBallSprites[index];
        }
    }

    private void Update()
    {
        if (isInputLocked) return;

        Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            defaultBall = null;
            if(ball.col == Physics2D.OverlapPoint(pos))
            {
                isDragging = true;
                defaultBall = ball;
                OnDragStart();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(defaultBall != null)
            {
                isDragging = false;
                OnDragEnd();
            }
        }
        if (isDragging)
        {
            OnDrag();
        }

        CheckGameOver();
    }

    #region Drag
    private void OnDragStart()
    {
        if (hasShot) return;

        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);

        trajectory.Show();
    }

    private void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * PushForce;

        trajectory.UpdateDots(defaultBall.pos, force);
    }

    private void OnDragEnd()
    {
        if (hasShot) return;

        defaultBall.Push(force);
        hasShot = true;

        trajectory.Hide();
    }
    #endregion

    private void CheckGameOver()
    {
        if (ball.pos.y <= -10) 
        {
            ShowGameOverCanvas();
        }
    }

    public void TogglePause()
    {
        if (pauseScreen == null) return;

        bool isPaused = pauseScreen.activeSelf;
        pauseScreen.SetActive(!isPaused);

        isInputLocked = !isInputLocked;
    }


    public void ShowGameOverCanvas()
    {
        if (hasScored || isGameOver) return;
        isGameOver = true;
        isInputLocked = true;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        gameOverCanvas.SetActive(true);
        StartCoroutine(FadeInCanvas(gameOverCanvasGroup));
    }

    private void ShowNextLevelCanvas()
    {
        isInputLocked = true;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        nextLevelCanvas.SetActive(true);
        StartCoroutine(FadeInCanvas(nextLevelCanvasGroup));
        DisplayStars();
    }

    private void ShowFinalCanvas()
    {
        isInputLocked = true;

        if(pauseCanvas != null)
            pauseCanvas.SetActive(false);

        finalCanvas.SetActive(true);
        StartCoroutine(FadeInCanvas(finalCanvasGroup));
        DisplayStars();
    }

    public void CollectCoin()
    {
        currentCoins++;

        if (audioEffects != null)
        {
            audioEffects.PickUpCoin();
        }
    }


    private void HandleScore()
    {
        hasScored = true;

        UnlockNextLevel();

        if(SceneManager.GetActiveScene().buildIndex == 9)
        {
            StartCoroutine(DelayFinalLevel());
        }
        else
        {
            StartCoroutine(DelayNextLevel());
        }
    }

    private void UnlockNextLevel()
    {
        if (currentCoins == totalCoins) 
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            int totalLevels = SceneManager.sceneCountInBuildSettings;

            if (currentLevel + 1 < totalLevels) 
            {
                PlayerPrefs.SetInt("LevelUnlocked_" + (currentLevel + 1), 1); 
                PlayerPrefs.Save();
                Debug.Log("Next level unlocked: Level " + (currentLevel + 1));
            }
        }
        else
        {
            Debug.Log("Next level not unlocked. Earn 3 stars to proceed.");
        }
    }


    private IEnumerator DelayNextLevel()
    {
        yield return new WaitForSeconds(1f);

        ShowNextLevelCanvas();
    }

    private IEnumerator DelayFinalLevel()
    {
        yield return new WaitForSeconds(1f);

        ShowFinalCanvas();
    }


    private void DisplayStars()
    {
        if (currentCoins <= totalCoins)
        {
            stars.sprite = starImages[currentCoins];
            Debug.Log("Stars Displayed: " + currentCoins);

            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            int previousMaxStars = PlayerPrefs.GetInt("Stars_Level_" + currentLevel, 0);

            if (currentCoins > previousMaxStars)
            {
                PlayerPrefs.SetInt("Stars_Level_" + currentLevel, currentCoins);
                PlayerPrefs.Save();
                Debug.Log($"New max stars for Level {currentLevel}: {currentCoins}");
            }
            else
            {
                Debug.Log($"Stars for Level {currentLevel} remain at max: {previousMaxStars}");
            }


            UpdateTotalStars();
        }
    }

    private void UpdateTotalStars()
    {
        int totalStars = 0;
        int totalLevels = SceneManager.sceneCountInBuildSettings; 

        for (int i = 1; i <= totalLevels; i++)
        {
            totalStars += PlayerPrefs.GetInt("Stars_Level_" + i, 0);
        }

        PlayerPrefs.SetInt("TotalStars", totalStars);
        PlayerPrefs.Save();
        Debug.Log("Total Stars Updated: " + totalStars);
    }

    public int GetTotalStars()
    {
        return PlayerPrefs.GetInt("TotalStars", 0);
    }


    private IEnumerator FadeInCanvas(CanvasGroup canvasGroup)
    {
        float duration = 1.0f;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    public bool HasScored()
    {
        return hasScored;
    }

    
}
