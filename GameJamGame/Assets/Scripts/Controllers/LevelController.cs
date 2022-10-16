using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
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

        yield return new WaitForSeconds(1f);
        FindObjectOfType<GridController>().CreateLevel(currentSize, currentSize);
        TimerController.Instance.continueTime();
        TimerController.Instance.RequestTime();
    }
}
