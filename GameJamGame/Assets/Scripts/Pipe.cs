using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField]
    public bool isOpenRight, isOpenLeft, isOpenUp, isOpenDown;

    [SerializeField]
    bool hasNext;
    [SerializeField]
    GameObject nextPipe;

    
    void Update()
    {
        //Da rimuovere nella build, solo test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowNext();
        }
    }

    public void ShowNext()
    {
        if (hasNext)
        {
            //TileController.Instance.TileObject = Instantiate(NextPipe, transform.position, Quaternion.identity);
            //Destroy(TileController.Instance.TileObject)
            Instantiate(nextPipe, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
