using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIGameController : MonoSingleton<UIGameController>
{
    [SerializeField] GameObject CurrentLevel;
    [SerializeField] GameObject Highscore;
    [SerializeField] GameObject PointsGained;
    [SerializeField] GameObject TimeUsed;
    int highScore = 0;

    private void updateHighscoreText(int pointsToAdd)
    {
        highScore += pointsToAdd;
        Highscore.GetComponent<TextMeshProUGUI>().text = highScore.ToString();
    }

    private void updateCurrentLevelText(int currentLevel)
    {
        CurrentLevel.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
    }

    private void updatePointsGainedText(int result)
    {        
        PointsGained.GetComponent<TextMeshProUGUI>().text = result.ToString();
    }

    private void TimeUsedText(int bonusScoreTime)
    {
        TimeUsed.GetComponent<TextMeshProUGUI>().text = bonusScoreTime.ToString();
    }

    public void VisualizePointsInfo(int currentLevel,  int baseScore, int timeSpent, int bonusScoreTime, int bonusScoreSparedPipes)
    {
        int result = baseScore + bonusScoreTime + bonusScoreSparedPipes;
        updateHighscoreText(result);
        updateCurrentLevelText(currentLevel);
        updatePointsGainedText(result);
        TimeUsedText(timeSpent);
    }   
}
