using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    GameObject[,] grid;
    public GameObject gridCell;
    public int height = 0;
    public int width = 0;
    public float cellSpawnOffset;

    void Awake()
    {
        //CreateGrid(); 
    }

    void Update()
    {
        
    }


    private void CreateGrid()
    {
        GameObject _cell;
        grid = new GameObject[height, width];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _cell = Instantiate(gridCell);
                grid[i, j] = _cell;
                _cell.transform.SetParent(transform, false);
                _cell.transform.position = new Vector3(cellSpawnOffset * i, cellSpawnOffset * j, 0);
                _cell.GetComponent<GridCell>().SetPosition(i, j);
            }
        }
    }
}
