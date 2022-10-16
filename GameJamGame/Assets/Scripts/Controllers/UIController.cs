using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuCanvas;
    [SerializeField] GameObject _settingsCanvas;
    [SerializeField] GameObject TitleParticles;
    [SerializeField] GameObject _creditsCanvas;
    [SerializeField] GameObject SettingsBG;
    [SerializeField] GameObject CreditsBG;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsCanvas.SetActive(true);
        TitleParticles.SetActive(false);
        SettingsBG.SetActive(true);
    }

    public void BackToMenu()
    {
        _mainMenuCanvas.SetActive(true);
        _settingsCanvas.SetActive(false);
        TitleParticles.SetActive(true);
        SettingsBG.SetActive(false);
        _creditsCanvas.SetActive(false);
        CreditsBG.SetActive(false);
    }

    public void OpenCredits()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsCanvas.SetActive(false);
        TitleParticles.SetActive(false);
        _creditsCanvas.SetActive(true);
        SettingsBG.SetActive(false);
        CreditsBG.SetActive(true);
    }
}
