using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    Vector2Int GridPosition;
    GameObject PipeSprite;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetPosition(int a, int b)
    {
        GridPosition = new Vector2Int(a, b);
    }
}
