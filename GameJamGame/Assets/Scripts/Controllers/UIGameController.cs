using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGameController : MonoSingleton<UIGameController>
{
    [SerializeField] GameObject Highscore;
    int highScore = 0;

    public void updateHighscoreText(int pointsToAdd)
    {
        highScore += pointsToAdd;
        Highscore.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = highScore.ToString();    
    }
}
