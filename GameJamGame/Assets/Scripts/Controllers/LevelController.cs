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
        TimerController.Instance.ResetTimer();
        FindObjectOfType<GridController>().CreateLevel(currentSize, currentSize);  
        //Setting the timer
        TimerController.Instance.timeToCount = 120;
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
        //Add Score
        Debug.Log("YOU WON THE LEVEL. 50 POINTS HAS BEEN ASSIGNED TO YOU");
        UIGameController.Instance.updateHighscoreText(50);
        int pathLength = FindObjectOfType<GridController>().pathLength - 1;
        int pipesUsed = FindObjectOfType<GridController>().howMuchPipesAreUsed();
        Debug.Log("Created Path length:" + pathLength + " while Pipes used: "+pipesUsed);
        TimerController.Instance.pauseTime();
        float timeElapsed = TimerController.Instance.timeElapsedFromRequest();
        Debug.Log("Time elapsed: "+timeElapsed);
        yield return new WaitForSeconds(4f);
        TimerController.Instance.AddTime(10);
        FindObjectOfType<GridController>().CreateLevel(currentSize, currentSize);
        TimerController.Instance.continueTime();
        TimerController.Instance.RequestTime();
    }
}
