using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int GridPosition;
    GameObject PipeSprite;
    public bool taken;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPipe(GameObject pipe)
    {
        PipeSprite = Instantiate(pipe, transform.position, Quaternion.identity);
        PipeSprite.transform.parent = gameObject.transform;
        PipeSprite.GetComponent<SpriteRenderer>().size =  new Vector2(0.43f, 0.43f);
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
}
