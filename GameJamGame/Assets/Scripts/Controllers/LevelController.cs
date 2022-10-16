using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelController : MonoSingleton<LevelController>
{
    [SerializeField]
    GameObject barrelPipe; 
    [SerializeField]
    GameObject dispenserPipe;
    int currentLevel = 0;
    [SerializeField]
    int currentSize = 5;
    [SerializeField]
    int incrementSize = 2;
    [SerializeField]
    int maxSize = 11;
    [SerializeField]
    int increaseEvery = 5;

    [SerializeField]
    GameObject[] leftGlass;

    [SerializeField]
    GameObject[] rightGlass;
    void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        barrelPipe.SetActive(false);
        dispenserPipe.SetActive(false);
        ScoreController.Instance.setupTimerTime();
        FindObjectOfType<GridController>().CreateLevel(currentSize, currentSize);  
        TimerController.Instance.StartTimer();
        TimerController.Instance.RequestTime();
    }

    public void SetNext()
    {
        barrelPipe.SetActive(false);
        dispenserPipe.SetActive(false);
        currentLevel++;
        if(currentLevel % increaseEvery == 0 && currentSize < maxSize)
        {
            FindObjectOfType<GridController>().resizeCamera();
            currentSize += incrementSize;
        }
    }

    public void Won()
    {
        SetNext();
        StartCoroutine(WonCoroutine());
        //barrelPipe.SetActive(false);
        //dispenserPipe.SetActive(false);
    }

    private IEnumerator WonCoroutine()
    {
        int pathLength = FindObjectOfType<GridController>().pathLength - 1;
        int pipesUsed = FindObjectOfType<GridController>().howMuchPipesAreUsed();
        TimerController.Instance.pauseTime();
        float timeElapsed = TimerController.Instance.timeElapsedFromRequest();
        ScoreController.Instance.assignScore(currentLevel,currentSize, currentSize,timeElapsed,pipesUsed, pathLength);
        List<GridCell> gc = FindObjectOfType<GridController>().getPathUser();
        //for (int i = 0; i < gc.Count; i++)
        //{
        //    gc[i].PipeSprite.GetComponent<Pipe>().Animate();
        //}
        yield return AnimationRoutine(gc);
        FindObjectOfType<GridController>().CreateLevel(currentSize, currentSize);
        TimerController.Instance.continueTime();
        TimerController.Instance.RequestTime();
    }

    IEnumerator AnimationRoutine(List<GridCell> list)
    {

        barrelPipe.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        FindObjectOfType<GridController>().startPipeInstance.GetComponent<fixepipe>().fill();
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < list.Count; i++)
        {
            list[i].PipeSprite.GetComponent<Pipe>().Animate();
            yield return new WaitForSeconds(0.2f);
        }
        FindObjectOfType<GridController>().endPipeInstance.GetComponent<fixepipe>().fill();

        yield return new WaitForSeconds(0.2f);
        dispenserPipe.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        leftGlass[2].SetActive(false);
        rightGlass[2].SetActive(false);
        leftGlass[1].SetActive(true);
        rightGlass[1].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        leftGlass[1].SetActive(false);
        rightGlass[1].SetActive(false);
        leftGlass[0].SetActive(true);
        rightGlass[0].SetActive(true);
        yield return new WaitForSeconds(1f);

        leftGlass[0].SetActive(false);
        rightGlass[0].SetActive(false);
        leftGlass[2].SetActive(true);
        rightGlass[2].SetActive(true);

        barrelPipe.SetActive(false);
        dispenserPipe.SetActive(false);
    }
}
