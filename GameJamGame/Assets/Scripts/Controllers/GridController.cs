using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoSingleton<GridController>
{

    public bool hasPickedUpPiece = false;
    public GameObject[,] grid;
    public GameObject gridCell;
    public int height = 0;
    public int width = 0;
    public float cellSpawnOffset;
    public GameObject[] Pipes;
    [SerializeField]
    Vector3 startingPoint;
    [SerializeField]
    List<GameObject> usedPipes;
    [SerializeField] 
    private Transform _cam;
    List<Vector2Int> pipePositions;

    Vector2Int startPosition;
    Vector2Int endPosition;

    void Awake()
    {
        
    }

    private void Start()
    {
        hasPickedUpPiece = false;
        CreateGrid();
        CreatePerfectPath((int)(grid.GetLength(0)* grid.GetLength(1))/3);
        //FindPath();
        //InsertPipe(pipePositions);
        CenterCamera();
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


    private void ResetGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j].GetComponent<GridCell>().GetComponent<SpriteRenderer>().color = Color.white;
                grid[i, j].GetComponent<GridCell>().taken = false;
            }
        }
    }

    void FindPath()
    {
        pipePositions = new List<Vector2Int>();
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

    void CreatePerfectPath(int tilesNumber)
    {
        tilesNumber = FindPerfectPath();
        while (tilesNumber != 12)
        {
            ResetGrid();
            tilesNumber = FindPerfectPath();
            Debug.Log(tilesNumber + " ," + pipePositions.Count);
        }
        InsertPipe(pipePositions);
    }

    int FindPerfectPath()
    {
        pipePositions = new List<Vector2Int>();
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
            if (yCurrentPosition < grid.GetLength(1) && yCurrentPosition >= 0)
            {
                nextCell = grid[nextPos.x, nextPos.y].GetComponent<GridCell>();
                nextCell.GetComponent<SpriteRenderer>().color = Color.red;
                nextCell.taken = true;
                nextPos = CheckNeighbours(nextCell);
                pipePositions.Add(nextPos);

            }
        }
        return pipePositions.Count;
        //InsertPipe(pipePositions);
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
                    startPosition = new Vector2Int(pipePositions[i].x, pipePositions[i].y);
                    usedPipes.Add(Pipes[0]);
                }
                else if (pipePositions[i].y == pipePositions[i + 1].y && pipePositions[i].x < pipePositions[i + 1].x)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[2]);
                    startPosition = new Vector2Int(pipePositions[i].x, pipePositions[i].y); 
                    usedPipes.Add(Pipes[2]);
                }
                else
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[1]);
                    startPosition = new Vector2Int(pipePositions[i].x, pipePositions[i].y);
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
        endPosition = new Vector2Int(pipePositions[pipePositions.Count - 2].x, pipePositions[pipePositions.Count - 2].y);
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

        grid[startPosition.x, startPosition.y].GetComponent<GridCell>().SetPipe(usedPipes[0]);
        grid[startPosition.x, startPosition.y].GetComponent<GridCell>().taken = true;
        grid[startPosition.x, startPosition.y].GetComponent<GridCell>().canBeClicked = false;
        grid[endPosition.x, endPosition.y].GetComponent<GridCell>().SetPipe(usedPipes[usedPipes.Count - 1]);
        grid[endPosition.x, endPosition.y].GetComponent<GridCell>().taken = true;
        grid[endPosition.x, endPosition.y].GetComponent<GridCell>().canBeClicked = false;

        for (int i = 1; i < usedPipes.Count - 1; i++)
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

    private void CenterCamera()
    {
        //Adjusting camera center and orthographic Size, according to grid size
        _cam.transform.position = new Vector3(grid[width / 2, height / 2].GetComponent<GridCell>().transform.position.x,
                                              grid[width / 2, height / 2].GetComponent<GridCell>().transform.position.y,
                                              _cam.transform.position.z);
        while (!CameraCanSee(grid[width - 1, height - 1].GetComponent<GridCell>().transform))
        {
            _cam.gameObject.GetComponent<Camera>().orthographicSize += 2;
        }
        _cam.gameObject.GetComponent<Camera>().orthographicSize += 1;



    }

    private bool CameraCanSee(Transform gameObject)
    {
        Camera camera = _cam.gameObject.GetComponent<Camera>();
        Vector3 res = camera.WorldToViewportPoint(gameObject.position);
        if (res.x >= 0 && res.x <= 1 &&
            res.y >= 0 && res.y <= 1 &&
            res.z > 0) return true;
        return false;
    }
}
