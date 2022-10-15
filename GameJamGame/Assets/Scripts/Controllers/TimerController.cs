using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoSingleton<TimerController>
{
    [SerializeField] Slider _slider;
    //[SerializeField] 
    public TextMeshProUGUI _text;
    public float timeToCount;
    private bool stopTimer;
    private Coroutine timerCoroutine;
    private void Start()
    {
    }
    public void ResetTimer()
    {
        if(timerCoroutine != null) StopCoroutine(timerCoroutine);
        stopTimer = false;
        _slider.maxValue = timeToCount;
        _slider.value = timeToCount;
    }
    public void StartTimer()
    {
        ResetTimer();
        float initialTime = Time.time;
        timerCoroutine = StartCoroutine(StartTimerCount(initialTime));
    }

    IEnumerator StartTimerCount(float initialTime)
    {
        while (!stopTimer)
        {
            float elapsedTime = Time.time - initialTime;
            float time = timeToCount - elapsedTime;
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time - minutes * 60f);
            if (time <= 0)
                stopTimer = true;
            else
            {
                _text.text = string.Format("{0:0}:{1:00}", minutes, seconds);
                _slider.value = time;
            }
            yield return null;
        }
    }
}
