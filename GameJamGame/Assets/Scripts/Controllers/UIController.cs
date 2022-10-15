using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuCanvas;
    [SerializeField] GameObject _settingsCanvas;

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
    }

    public void BackToMenu()
    {
        _mainMenuCanvas.SetActive(true);
        _settingsCanvas.SetActive(false);
    }
}
