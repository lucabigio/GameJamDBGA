using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoSingleton<LevelController>
{
    
    void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        TimerController.Instance.ResetTimer();
        FindObjectOfType<GridController>().CreateLevel(7, 7);  
        //Setting the timer
        TimerController.Instance.timeToCount = 120;
        TimerController.Instance.StartTimer();
    }

    public void Won()
    {
        StartCoroutine(WonCoroutine());
    }

    private IEnumerator WonCoroutine()
    {
        //Add Score
        Debug.Log("YOU WON THE LEVEL. 50 POINTS HAS BEEN ASSIGNED TO YOU");
        UIGameController.Instance.updateHighscoreText(50);
        yield return new WaitForSeconds(1f);
        FindObjectOfType<GridController>().CreateLevel(7, 7);
    }
}
