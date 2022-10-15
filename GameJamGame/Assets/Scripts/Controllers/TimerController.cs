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
    private bool endTimer;
    private bool pauseTimer;
    private Coroutine timerCoroutine;
    private float initialTime;
    private float pauseStart;
    private float requestTime;

    private void Start()
    {
    }
    public void ResetTimer()
    {
        if(timerCoroutine != null) StopCoroutine(timerCoroutine);
        endTimer = false;
        pauseTimer = false;
        _slider.maxValue = timeToCount;
        _slider.value = timeToCount;
    }
    public void StartTimer()
    {
        ResetTimer();
        requestTime = Time.time;
        initialTime = Time.time;
        timerCoroutine = StartCoroutine(StartTimerCount());
    }

    public void AddTime(float secondsToAdd)
    {
        timeToCount += secondsToAdd;
        _slider.maxValue = timeToCount;
    }

    public void pauseTime()
    {
        pauseTimer = true;
        pauseStart = Time.time;
    }
    public void continueTime()
    {
        float elapsedFromPause = Time.time - pauseStart;
        pauseTimer = false;
        initialTime += elapsedFromPause;
    }

    IEnumerator StartTimerCount()
    {
        while (!endTimer)
        {
            if (!pauseTimer)
            {
                float elapsedTime = Time.time - initialTime;
                float time = timeToCount - elapsedTime;
                int minutes = Mathf.FloorToInt(time / 60);
                int seconds = Mathf.FloorToInt(time - minutes * 60f);
                if (time <= 0)
                    endTimer = true;
                else
                {
                    _text.text = string.Format("{0:0}:{1:00}", minutes, seconds);
                    _slider.value = time;
                }
            }
            yield return null;
        }
    }

    public float timeElapsedFromRequest()
    {
        return Time.time - requestTime;
    }

    public void RequestTime()
    {
        requestTime = Time.time;
    }
}
