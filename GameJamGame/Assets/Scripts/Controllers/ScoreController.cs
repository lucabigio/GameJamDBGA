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

    public void assignScore(int width, int height, float timeSpent, int pipesUsed, int pipesPath)
    {
        int cellsGrid = width * height;
        int baseScore = (int)Math.Round(multiplierBaseScorePerLevel * cellsGrid, MidpointRounding.AwayFromZero);
        float expectedTime = multiplierExpectedTimeToSolve * pipesPath;
        int secondsToAddTimer = (int)Math.Round(multiplierSecondsGivenForLevel * expectedTime, MidpointRounding.AwayFromZero);
        float sparedSeconds = expectedTime - timeSpent;
        int bonusScoreTime = 0;
        if (sparedSeconds >= 1) bonusScoreTime = (int)Math.Round(bonusPointsScorePerTime * sparedSeconds, MidpointRounding.AwayFromZero);
        int sparedPipes = pipesPath - pipesUsed;
        int bonusScoreSparedPipes = 0;
        if (sparedPipes >= 1) bonusScoreSparedPipes = (int)Math.Round(bonusPointsScorePerPipes * sparedPipes, MidpointRounding.AwayFromZero);

    }
}
