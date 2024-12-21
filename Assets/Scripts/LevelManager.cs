using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    private CanvasGroup nextLevelCanvasGroup;
    private CanvasGroup gameOverCanvasGroup;

    public GameObject pauseCanvas;

    public List<Sprite> starImages; 
    public Image stars;

    Scoring scoring;

    private bool isInputLocked = false;

    private bool hasScored = false;

    private bool isGameOver = false;

    private bool hasShot = false;


    private void Start()
    {   
        scoring = FindAnyObjectByType<Scoring>();

        cam = Camera.main;

        if (nextLevelCanvas != null)
            nextLevelCanvasGroup = nextLevelCanvas.GetComponent<CanvasGroup>();
        if (gameOverCanvas != null)
            gameOverCanvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();

        int selectedBackgroundIndex = PlayerPrefs.GetInt("SelectedBackground", 0);
        SetGameBackground(selectedBackgroundIndex);

        int selectedBallIndex = PlayerPrefs.GetInt("SelectedBall", 0);
        SetGameBall(selectedBallIndex);


        scoring.OnScored += HandleScore;
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

    public void CollectCoin()
    {
        currentCoins++;
        AudioManager.Instance.PlayCoinPickupSound();
    }


    private void HandleScore()
    {
        hasScored = true;
        StartCoroutine(DelayNextLevel());
    }

    private IEnumerator DelayNextLevel()
    {
        yield return new WaitForSeconds(1f);

        ShowNextLevelCanvas();
    }


    private void DisplayStars()
    {
        if(currentCoins <= totalCoins)
        {
            stars.sprite = starImages[currentCoins];
            Debug.Log("Stars Displayed: " + currentCoins);
        }

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
