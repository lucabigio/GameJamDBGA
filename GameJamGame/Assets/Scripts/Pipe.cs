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

    public bool CanBeMoved;
    Vector3 mousePosition;

    void FixedUpdate()
    {
        //Da rimuovere nella build, solo test
        if (CanBeMoved)
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, 1);
        }
    }

    public GameObject ShowNext()
    {
        if (hasNext)
        {
            //TileController.Instance.TileObject = Instantiate(NextPipe, transform.position, Quaternion.identity);
            //Destroy(TileController.Instance.TileObject)
            //Instantiate(nextPipe, transform.position, Quaternion.identity);
            //Destroy(gameObject);
            return nextPipe;
        }
        return null;
    }

    
}
