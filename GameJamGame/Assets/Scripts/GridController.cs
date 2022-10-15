using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    GameObject[,] grid;
    public GameObject gridCell;
    public int height = 0;
    public int width = 0;
    public float cellSpawnOffset;
    public GameObject[] Pipes;
    [SerializeField]
    Vector3 startingPoint;
    [SerializeField]
    List<GameObject> usedPipes;
    void Awake()
    {
        
        CreateGrid();
        FindPath();
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
                _cell.transform.position = startingPoint;
                _cell.transform.position += new Vector3(cellSpawnOffset * i, cellSpawnOffset * j, 0);
                _cell.GetComponent<GridCell>().SetPosition(i, j);
            }
        }
    }

    void FindPath()
    {
        List<Vector2Int> pipePositions = new List<Vector2Int>();
        int yCurrentPosition = 0;
        GridCell nextCell = grid[Random.Range(0, grid.GetLength(0)), 0].GetComponent<GridCell>();
        nextCell.GetComponent<SpriteRenderer>().color = Color.red;
        nextCell.taken = true;
        pipePositions.Add(nextCell.GetPosition());
        Vector2Int nextPos = CheckNeighbours(nextCell);
        pipePositions.Add(nextPos);
        while (yCurrentPosition < grid.GetLength(1) && yCurrentPosition >= 0)
        {
            yCurrentPosition = nextPos.y;
            if(yCurrentPosition < grid.GetLength(1) && yCurrentPosition >= 0)
            {
                nextCell = grid[nextPos.x, nextPos.y].GetComponent<GridCell>();
                nextCell.GetComponent<SpriteRenderer>().color = Color.red;
                nextCell.taken = true;
                nextPos = CheckNeighbours(nextCell);
                pipePositions.Add(nextPos);
                
            }
        }
        InsertPipe(pipePositions);
        
    }
    private void InsertPipe(List<Vector2Int> pipePositions)
    {
        usedPipes = new List<GameObject>();
        for (int i = 0; i < pipePositions.Count - 2; i++)
        {
            if (i == 0)
            {
                if (pipePositions[i].y < pipePositions[i + 1].y && pipePositions[i].x == pipePositions[i + 1].x)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[0]);
                    usedPipes.Add(Pipes[0]);
                }
                else if (pipePositions[i].y == pipePositions[i + 1].y && pipePositions[i].x < pipePositions[i + 1].x)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[2]);
                    usedPipes.Add(Pipes[2]);
                }
                else
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[1]);
                    usedPipes.Add(Pipes[1]);
                }
            }
            else if (pipePositions[i].y > pipePositions[i - 1].y && pipePositions[i].x == pipePositions[i - 1].x)
            {
                if (pipePositions[i].x == pipePositions[i + 1].x)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[0]);
                    usedPipes.Add(Pipes[0]);
                }
                if (pipePositions[i].x < pipePositions[i + 1].x)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[2]);
                    usedPipes.Add(Pipes[2]);
                }
                if (pipePositions[i].x > pipePositions[i + 1].x)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[1]);
                    usedPipes.Add(Pipes[1]);
                }
            }
            else if (pipePositions[i].y == pipePositions[i - 1].y && pipePositions[i].x < pipePositions[i - 1].x)
            {
                if (pipePositions[i].y == pipePositions[i + 1].y)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[3]);
                    usedPipes.Add(Pipes[3]);
                }
                if (pipePositions[i].y < pipePositions[i + 1].y)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[5]);
                    usedPipes.Add(Pipes[5]);
                }
            }
            else if (pipePositions[i].y == pipePositions[i - 1].y && pipePositions[i].x > pipePositions[i - 1].x)
            {
                if (pipePositions[i].y == pipePositions[i + 1].y)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[3]);
                    usedPipes.Add(Pipes[3]);
                }
                if (pipePositions[i].y < pipePositions[i + 1].y)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[4]);
                    usedPipes.Add(Pipes[4]);
                }
            }
        }
        grid[pipePositions[pipePositions.Count - 2].x, pipePositions[pipePositions.Count - 2].y].GetComponent<GridCell>().SetPipe(Pipes[0]);
        usedPipes.Add(Pipes[0]);
        RandomizePipes();
    }

    private void RandomizePipes()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[i, j].GetComponent<GridCell>().taken)
                {
                    grid[i, j].GetComponent<GridCell>().ClearCell();
                }
            }
        }
        for(int i = 0; i < usedPipes.Count; i++)
        {
            bool foundPlace = false;
            while(!foundPlace)
            {
                Vector2Int index = new Vector2Int(Random.Range(0, grid.GetLength(0)), Random.Range(0, grid.GetLength(1)));
                if (!grid[index.x, index.y].GetComponent<GridCell>().taken)
                {
                    grid[index.x, index.y].GetComponent<GridCell>().SetPipe(usedPipes[i]);
                    grid[index.x, index.y].GetComponent<GridCell>().taken = true;
                    foundPlace = true;
                }
            }
        }
    }

    private Vector2Int CheckNeighbours(GridCell nextCell)
    {
        List<Vector2Int> freeCells = new List<Vector2Int>();
        int[] possiblePipeIndex = new int[3];
        if(nextCell.GetPosition().y == grid.GetLength(1) - 1)
        {
            return new Vector2Int(-1, -1);
        }
        if(nextCell.GetPosition().x - 1 >= 0 && nextCell.GetPosition().y < grid.GetLength(1))
        {
            if (!grid[nextCell.GetPosition().x - 1, nextCell.GetPosition().y].GetComponent<GridCell>().taken)
            {
                freeCells.Add(new Vector2Int(nextCell.GetPosition().x - 1, nextCell.GetPosition().y));
            }
        }
        if(nextCell.GetPosition().x + 1 < grid.GetLength(0) && nextCell.GetPosition().y < grid.GetLength(1))
        {
            if(!grid[nextCell.GetPosition().x + 1, nextCell.GetPosition().y].GetComponent<GridCell>().taken)
            {
                freeCells.Add(new Vector2Int(nextCell.GetPosition().x + 1, nextCell.GetPosition().y));
                possiblePipeIndex[1] = 2;
            }
        }
        if (nextCell.GetPosition().y + 1 < grid.GetLength(1))
        {
            if (!grid[nextCell.GetPosition().x, nextCell.GetPosition().y + 1].GetComponent<GridCell>().taken)
            {
                freeCells.Add(new Vector2Int(nextCell.GetPosition().x, nextCell.GetPosition().y + 1));
                possiblePipeIndex[2] = 3;
            }
        }


        int randomIndex = Random.Range(0, freeCells.Count);


        if (freeCells.Count > 0)
        {
            return freeCells[randomIndex];
        }
        return new Vector2Int(-1, -1);
    }
}
