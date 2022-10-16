using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class LevelController : MonoSingleton<LevelController>
{
    int currentLevel = 0;
    [SerializeField]
    int currentSize = 5;
    [SerializeField]
    int incrementSize = 2;
    [SerializeField]
    int maxSize = 11;
    [SerializeField]
    int increaseEvery = 5;
    public GameObject deathPanel;
    public GameObject scoreTextInDeathPanel;
    void Start()
    {
        AudioController.Instance.PlayMusic(4, true);
        StartLevel();
    }

    public void Lose()
    {
        FindObjectOfType<GridController>().DestroyGrid();
        deathPanel.SetActive(true);
        scoreTextInDeathPanel.GetComponent<TextMeshProUGUI>().text = UIGameController.Instance.highScore.ToString();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        UnityEngine.Application.Quit();
    }

    public void RestartGame()
    {
        deathPanel.SetActive(false);
        UIGameController.Instance.highScore = 0;
        UIGameController.Instance.VisualizePointsInfo(0, 0, 0, 0, 0);
        StartLevel();
    }
    public void StartLevel()
    {
        ScoreController.Instance.setupTimerTime();
        FindObjectOfType<GridController>().CreateLevel(currentSize, currentSize);  
        TimerController.Instance.StartTimer();
        TimerController.Instance.RequestTime();
    }

    public void SetNext()
    {
        currentLevel++;
        if(currentLevel % increaseEvery == 0 && currentSize < maxSize)
        {
            currentSize += incrementSize;
        }
    }

    public void Won()
    {
        SetNext();
        StartCoroutine(WonCoroutine());
    }

    private IEnumerator WonCoroutine()
    {
        int pathLength = FindObjectOfType<GridController>().pathLength - 1;
        int pipesUsed = FindObjectOfType<GridController>().howMuchPipesAreUsed();
        TimerController.Instance.pauseTime();
        float timeElapsed = TimerController.Instance.timeElapsedFromRequest();
        ScoreController.Instance.assignScore(currentLevel,currentSize, currentSize,timeElapsed,pipesUsed, pathLength);
        List<GridCell> gc = FindObjectOfType<GridController>().getPathUser();
        for (int i = 0; i < gc.Count; i++)
        {
            //Animations Should Start Here
        }
        yield return new WaitForSeconds(1f);
        FindObjectOfType<GridController>().CreateLevel(currentSize, currentSize);
        TimerController.Instance.continueTime();
        TimerController.Instance.RequestTime();
    }
}
