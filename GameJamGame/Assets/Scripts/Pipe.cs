using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField]
    public bool isOpenRight, isOpenLeft, isOpenUp, isOpenDown;
    [SerializeField]
    public Sprite[] possibleSprites;
    [SerializeField]
    bool hasNext;
    [SerializeField]
    GameObject nextPipe;

    public bool CanBeMoved;
    Vector3 mousePosition;
    public int spriteIndex;
    Animator animator;
    public bool hasCurrent { get; set; }
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        hasCurrent = false;
    }

    public void Animate()
    {
        animator.enabled = true;
    }
    public void SetSprite(int n)
    {
        spriteIndex = n;
        GetComponent<SpriteRenderer>().sprite = possibleSprites[n];
    }

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
            GameObject pipeToShow = nextPipe;
            pipeToShow.GetComponent<Pipe>().SetSprite(spriteIndex);
            //TileController.Instance.TileObject = Instantiate(NextPipe, transform.position, Quaternion.identity);
            //Destroy(TileController.Instance.TileObject)
            //Instantiate(nextPipe, transform.position, Quaternion.identity);
            //Destroy(gameObject);
            return pipeToShow;
        }
        return null;
    }

    
}
