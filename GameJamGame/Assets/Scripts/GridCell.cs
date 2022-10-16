using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int GridPosition;
    public GameObject PipeSprite;
    public bool taken;
    public GameObject lastPipe;
    public bool canBeClicked = true;
    public bool canRotate = true;
    [SerializeField] GameObject _highlight;
    //[SerializeField] GameObject _cantMoved;
    //[SerializeField] GameObject _canOnlyRotate;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setCantMove()
    {
        PipeSprite.GetComponent<Pipe>().SetSprite(1);
        //_cantMoved.SetActive(true);
    }
    public void setOnlyRotation()
    {
        PipeSprite.GetComponent<Pipe>().SetSprite(2);
        //_cantMoved.SetActive(false);
        //_canOnlyRotate.SetActive(true);
    }
    public void SetPipe(GameObject pipe)
    {
        //taken = true;
        PipeSprite = Instantiate(pipe, transform.position, Quaternion.identity);
        PipeSprite.transform.parent = gameObject.transform;
        PipeSprite.GetComponent<SpriteRenderer>().size =  new Vector2(1, 1);
        if(canBeClicked && canRotate)
        {
            PipeSprite.GetComponent<Pipe>().SetSprite(0);
        }
        else if (!canBeClicked && !canRotate)
        {
            PipeSprite.GetComponent<Pipe>().SetSprite(1);
        }
        else if (!canBeClicked && canRotate)
        {
            PipeSprite.GetComponent<Pipe>().SetSprite(2);
        }
        lastPipe = PipeSprite;
    }

    public void SetPosition(int a, int b)
    {
        GridPosition = new Vector2Int(a, b);
    }

    public Vector2Int GetPosition()
    {
        return GridPosition;
    }

    public void ClearCell()
    {
        Destroy(PipeSprite);
        PipeSprite = null;
        taken = false;
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
    private void OnMouseOver()
    {
        _highlight.SetActive(true);
        if (canBeClicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (taken)
                {
                    if (GetComponentInParent<GridController>().hasPickedUpPiece)
                    {
                        GameObject thisPipe = PipeSprite;
                        lastPipe = GameObject.FindGameObjectWithTag("DraggablePipe");
                        SetPipe(lastPipe);
                        taken = true;
                        lastPipe.GetComponent<Pipe>().CanBeMoved = false;
                        GameObject clearMouse = GameObject.FindGameObjectWithTag("DraggablePipe");
                        if (clearMouse)
                        {
                            Destroy(clearMouse);
                        }
                        lastPipe.tag = "Pipe";

                        thisPipe.GetComponent<Pipe>().CanBeMoved = true;
                        thisPipe.transform.parent = null;
                        //thisPipe.GetComponent<GridCell>().taken = false;
                        thisPipe.tag = "DraggablePipe";
                        GetComponentInParent<GridController>().hasPickedUpPiece = true;

                    }
                    else
                    {
                        PipeSprite.GetComponent<Pipe>().CanBeMoved = true;
                        PipeSprite.transform.parent = null;
                        taken = false;
                        PipeSprite.tag = "DraggablePipe";
                        GetComponentInParent<GridController>().hasPickedUpPiece = true;
                    }
                }
                else
                {
                    lastPipe = GameObject.FindGameObjectWithTag("DraggablePipe");
                    if (lastPipe)
                    {
                        SetPipe(lastPipe);
                        taken = true;
                        lastPipe.GetComponent<Pipe>().CanBeMoved = false;
                        GameObject clearMouse = GameObject.FindGameObjectWithTag("DraggablePipe");
                        if (clearMouse)
                        {
                            Destroy(clearMouse);
                        }
                        GetComponentInParent<GridController>().hasPickedUpPiece = false;
                        lastPipe.tag = "Pipe";
                    }
                }
                if (FindObjectOfType<GridController>().DoIWon()) LevelController.Instance.Won();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (taken)
                {
                    GameObject newPipe = PipeSprite;
                    Destroy(PipeSprite);
                    PipeSprite = null;
                    SetPipe(newPipe.GetComponent<Pipe>().ShowNext());
                }
                if (FindObjectOfType<GridController>().DoIWon()) LevelController.Instance.Won();
            }
        }
        else if (canRotate)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (taken)
                {
                    GameObject newPipe = PipeSprite;
                    Destroy(PipeSprite);
                    PipeSprite = null;
                    SetPipe(newPipe.GetComponent<Pipe>().ShowNext());
                }
                if (FindObjectOfType<GridController>().DoIWon()) LevelController.Instance.Won();
            }
        }
    }
}
