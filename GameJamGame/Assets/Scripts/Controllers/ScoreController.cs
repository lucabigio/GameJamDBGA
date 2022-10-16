using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoSingleton<ScoreController>
{
    public float timerBaseTime = 120f;
    public float multiplierBaseScorePerLevel = 2f;
    public float bonusPointsScorePerTime = 5f;
    public float bonusPointsScorePerPipes = 5f;
    public float multiplierExpectedTimeToSolve = 1.5f;
    public float multiplierSecondsGivenForLevel = 1.5f;

    public void setupTimerTime()
    {
        TimerController.Instance.ResetTimer();
        TimerController.Instance.timeToCount = timerBaseTime;
    }

    public void assignScore(int currentLevel, int width, int height, float timeSpent, int pipesUsed, int pipesPath)
    {
        int cellsGrid = width * height;
        int baseScore = (int)Math.Round(multiplierBaseScorePerLevel * cellsGrid, MidpointRounding.AwayFromZero);
        float expectedTime = multiplierExpectedTimeToSolve * pipesPath;
        int secondsToAddTimer = (int)Math.Round(multiplierSecondsGivenForLevel * expectedTime, MidpointRounding.AwayFromZero);

        TimerController.Instance.AddTime(secondsToAddTimer);

        float sparedSeconds = expectedTime - timeSpent;
        int roundedTimeSpent = (int)Math.Round(timeSpent, MidpointRounding.AwayFromZero);
        int bonusScoreTime = 0;
        if (sparedSeconds >= 1) bonusScoreTime = (int)Math.Round(bonusPointsScorePerTime * sparedSeconds, MidpointRounding.AwayFromZero);
        int sparedPipes = pipesPath - pipesUsed;
        int bonusScoreSparedPipes = 0;
        if (sparedPipes >= 1) bonusScoreSparedPipes = (int)Math.Round(bonusPointsScorePerPipes * sparedPipes, MidpointRounding.AwayFromZero);
        UIGameController.Instance.VisualizePointsInfo(currentLevel, baseScore, roundedTimeSpent, bonusScoreTime, bonusScoreSparedPipes);

        Debug.Log("cellsGrid:" + cellsGrid);
        Debug.Log("baseScore:" + baseScore);
        Debug.Log("expectedTime:" + expectedTime);
        Debug.Log("secondsToAddTimer:" + secondsToAddTimer);
        Debug.Log("TimeSpent:" + roundedTimeSpent);
        Debug.Log("sparedSeconds:" + sparedSeconds);
        Debug.Log("bonusScoreTime:" + bonusScoreTime);
        Debug.Log("sparedPipes:" + sparedPipes);
        Debug.Log("bonusScoreSparedPipes:" + bonusScoreSparedPipes);



    }

}
