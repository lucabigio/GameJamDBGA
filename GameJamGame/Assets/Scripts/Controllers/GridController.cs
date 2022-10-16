using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

enum FlowDirection
{
    Top,
    Left,
    Right,
    Bottom
}

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
    List<GridCell> notMovingCells;
    public int pathLength { get; set; }
    int notMovingTileNumber;
    Vector2Int[] notMovingIndexes;
    Vector2Int startPosition;
    Vector2Int endPosition;

    void Awake()
    {
        
    }

    private void Start()
    {
        /*hasPickedUpPiece = false;
        CreateGrid();
        CreatePerfectPath((int)(grid.GetLength(0)* grid.GetLength(1))/3);
        //FindPath();
        //InsertPipe(pipePositions);
        CenterCamera();*/
        //CreateLevel(height, width);
    }

    

    void Update()
    {
        
    }

    public void CreateLevel(int _h, int _w)
    {
        hasPickedUpPiece = false;
        if (grid != null && grid.Length > 0)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Destroy(grid[i, j].GetComponent<GridCell>().PipeSprite);
                    Destroy(grid[i, j]);
                }
            }
            System.Array.Clear(grid, 0, grid.Length);
        }
        height = _h;
        width = _w;
        CreateGrid();
        pathLength = Random.Range( 2 * height, 3 * height  );
        //pathLength = (int)(grid.GetLength(0) * grid.GetLength(1)) / 3;
        CreatePerfectPath(pathLength);
        _cam.gameObject.GetComponent<Camera>().orthographicSize -= 1;
        CenterCamera();
        if (DoIWon()) LevelController.Instance.Won();
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
        //nextCell.GetComponent<SpriteRenderer>().color = Color.red;
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
                //nextCell.GetComponent<SpriteRenderer>().color = Color.red;
                nextCell.taken = true;
                nextPos = CheckNeighbours(nextCell);
                pipePositions.Add(nextPos);
                
            }
        }
        InsertPipe(pipePositions);
        
    }

    void CreatePerfectPath(int tilesNumber)
    {
        notMovingTileNumber = (tilesNumber - 2) / 2;
        int tileUsed = FindPerfectPath();
        notMovingIndexes = new Vector2Int[notMovingTileNumber];
        while (tileUsed != tilesNumber)
        {
            ResetGrid();
            tileUsed = FindPerfectPath();
            //Debug.Log(tilesNumber + " ," + pipePositions.Count);
        }
        for(int i = 0; i < notMovingIndexes.Length; i++)
        {
            int randIndex = Random.Range(1, pipePositions.Count - 2);

            if(!ExistInArray(pipePositions[randIndex], notMovingIndexes))
            {
                notMovingIndexes[i] = pipePositions[randIndex];
            }
            //Debug.Log(notMovingIndexes[i]);
        }
        //notMovingTileNumber = (tilesNumber - 2) / notMovingTileNumber;
        InsertPipe(pipePositions);
    }
    bool ExistInArray(Vector2Int listElement, Vector2Int[] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if(listElement == array[i])
            {
                return true;
            }
        }
        return false;
    }
    int FindPerfectPath()
    {
        pipePositions = new List<Vector2Int>();
        int yCurrentPosition = 0;
        GridCell nextCell = grid[Random.Range(0, grid.GetLength(0)), 0].GetComponent<GridCell>();
        //nextCell.GetComponent<SpriteRenderer>().color = Color.red;
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
                //nextCell.GetComponent<SpriteRenderer>().color = Color.red;
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
        notMovingCells = new List<GridCell>();
        usedPipes = new List<GameObject>();
        for (int i = 0; i < pipePositions.Count - 2; i++)
        {
            if (i == 0)
            {
                if (pipePositions[i].y < pipePositions[i + 1].y && pipePositions[i].x == pipePositions[i + 1].x)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[0]);
                    startPosition = new Vector2Int(pipePositions[i].x, pipePositions[i].y);
                    //usedPipes.Add(Pipes[0]);
                    if (ExistInArray(pipePositions[i], notMovingIndexes))
                    {
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().canBeClicked = false;
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().setCantMove();
                    }
                    usedPipes.Add(Pipes[0]);
                }
                else if (pipePositions[i].y == pipePositions[i + 1].y && pipePositions[i].x < pipePositions[i + 1].x)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[2]);
                    startPosition = new Vector2Int(pipePositions[i].x, pipePositions[i].y); 
                    //usedPipes.Add(Pipes[2]);
                    if (ExistInArray(pipePositions[i], notMovingIndexes))
                    {
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().canBeClicked = false;
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().setCantMove();

                    }
                    usedPipes.Add(Pipes[2]);
                }
                else
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[1]);
                    startPosition = new Vector2Int(pipePositions[i].x, pipePositions[i].y);
                    //usedPipes.Add(Pipes[1]);
                    if (ExistInArray(pipePositions[i], notMovingIndexes))
                    {
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().canBeClicked = false;
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().setCantMove();
                    }
                    usedPipes.Add(Pipes[1]);
                }
            }
            else if (pipePositions[i].y > pipePositions[i - 1].y && pipePositions[i].x == pipePositions[i - 1].x)
            {
                if (pipePositions[i].x == pipePositions[i + 1].x)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[0]);
                    //usedPipes.Add(Pipes[0]);
                    if (ExistInArray(pipePositions[i], notMovingIndexes))
                    {
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().canBeClicked = false;
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().setCantMove();
                        notMovingCells.Add(grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>());
                    }
                    else
                    {
                        usedPipes.Add(Pipes[0]);
                    }
                }
                if (pipePositions[i].x < pipePositions[i + 1].x)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[2]);
                    //usedPipes.Add(Pipes[2]);
                    if (ExistInArray(pipePositions[i], notMovingIndexes))
                    {
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().canBeClicked = false;
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().setCantMove();
                        notMovingCells.Add(grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>());
                    }
                    else
                    {
                        usedPipes.Add(Pipes[2]);
                    }
                }
                if (pipePositions[i].x > pipePositions[i + 1].x)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[1]);
                    //usedPipes.Add(Pipes[1]);
                    if (ExistInArray(pipePositions[i], notMovingIndexes))
                    {
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().canBeClicked = false;
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().setCantMove();
                        notMovingCells.Add(grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>());
                    }
                    else
                    {
                        usedPipes.Add(Pipes[1]);
                    }
                }
            }
            else if (pipePositions[i].y == pipePositions[i - 1].y && pipePositions[i].x < pipePositions[i - 1].x)
            {
                if (pipePositions[i].y == pipePositions[i + 1].y)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[3]);
                    //usedPipes.Add(Pipes[3]);
                    if (ExistInArray(pipePositions[i], notMovingIndexes))
                    {
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().canBeClicked = false;
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().setCantMove();
                        notMovingCells.Add(grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>());
                    }
                    else
                    {
                        usedPipes.Add(Pipes[3]);
                    }
                }
                if (pipePositions[i].y < pipePositions[i + 1].y)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[5]);
                    //usedPipes.Add(Pipes[5]);
                    if (ExistInArray(pipePositions[i], notMovingIndexes))
                    {
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().canBeClicked = false;
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().setCantMove();
                        notMovingCells.Add(grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>());
                    }
                    else
                    {
                        usedPipes.Add(Pipes[5]);
                    }
                }
            }
            else if (pipePositions[i].y == pipePositions[i - 1].y && pipePositions[i].x > pipePositions[i - 1].x)
            {
                if (pipePositions[i].y == pipePositions[i + 1].y)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[3]);
                    //usedPipes.Add(Pipes[3]);
                    if (ExistInArray(pipePositions[i], notMovingIndexes))
                    {
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().canBeClicked = false;
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().setCantMove();
                        notMovingCells.Add(grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>());
                    }
                    else
                    {
                        usedPipes.Add(Pipes[3]);
                    }
                }
                if (pipePositions[i].y < pipePositions[i + 1].y)
                {
                    grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().SetPipe(Pipes[4]);
                    //usedPipes.Add(Pipes[4]);
                    if (ExistInArray(pipePositions[i], notMovingIndexes))
                    {
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().canBeClicked = false;
                        grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>().setCantMove();
                        notMovingCells.Add(grid[pipePositions[i].x, pipePositions[i].y].GetComponent<GridCell>());
                    }
                    else
                    {
                        usedPipes.Add(Pipes[4]);
                    }
                }
            }
        }
        grid[pipePositions[pipePositions.Count - 2].x, pipePositions[pipePositions.Count - 2].y].GetComponent<GridCell>().SetPipe(Pipes[0]);
        grid[pipePositions[pipePositions.Count - 2].x, pipePositions[pipePositions.Count - 2].y].GetComponent<GridCell>().taken = true;
        grid[pipePositions[pipePositions.Count - 2].x, pipePositions[pipePositions.Count - 2].y].GetComponent<GridCell>().canBeClicked = false;
        endPosition = new Vector2Int(pipePositions[pipePositions.Count - 2].x, pipePositions[pipePositions.Count - 2].y);
        //usedPipes.Add(Pipes[0]);
        //Debug.Log(notMovingCells.Count);
        
        RandomizePipes();
    }

    private void RandomizePipes()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[i, j].GetComponent<GridCell>().taken && grid[i, j].GetComponent<GridCell>().canBeClicked)
                {
                    grid[i, j].GetComponent<GridCell>().ClearCell();
                }
            }
        }

        grid[startPosition.x, startPosition.y].GetComponent<GridCell>().SetPipe(usedPipes[0]);
        grid[startPosition.x, startPosition.y].GetComponent<GridCell>().taken = true;
        grid[startPosition.x, startPosition.y].GetComponent<GridCell>().canBeClicked = false;

        //Debug.Log(usedPipes.Count);
        //grid[endPosition.x, endPosition.y].GetComponent<GridCell>().SetPipe(usedPipes[usedPipes.Count - 2]);
        //grid[endPosition.x, endPosition.y].GetComponent<GridCell>().taken = true;
        //grid[endPosition.x, endPosition.y].GetComponent<GridCell>().canBeClicked = false;

        for (int i = 1; i < usedPipes.Count; i++)
        {
            bool foundPlace = false;
            while (!foundPlace)
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
        int onlyRotatingCellsNumber = notMovingCells.Count / 2;
        for (int i = 0; i < onlyRotatingCellsNumber; i++)
        {
            int ranIndex = Random.Range(0, notMovingCells.Count);
            notMovingCells[ranIndex].canRotate = true;
            notMovingCells[ranIndex].setOnlyRotation();
            for (int x = 0; x < Random.Range(0, 3); x++)
            {
                //notMovingCells[ranIndex].PipeSprite.GetComponent<Pipe>().ShowNext();
                Destroy(notMovingCells[ranIndex].PipeSprite.GetComponent<Pipe>().gameObject);
                notMovingCells[ranIndex].SetPipe(notMovingCells[ranIndex].PipeSprite.GetComponent<Pipe>().ShowNext());
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

    private bool CheckVictory(Vector2Int position, FlowDirection flowDir) {
        if(position.x < 0 || position.y < 0 || position.x >= width || position.y >= height) return false;

        GameObject pipeSprite = grid[position.x, position.y].GetComponent<GridCell>().PipeSprite;
        if (pipeSprite != null)
        {
            Pipe attachedPipe = pipeSprite.GetComponent<Pipe>();
            switch (flowDir)
            {
                case FlowDirection.Top:
                    if (attachedPipe.isOpenUp)
                    {
                        //pipeSprite.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.5f);
                        attachedPipe.hasCurrent = true;
                        bool won = false;
                        //Dai corrente nelle altre direzioni
                        if (attachedPipe.isOpenDown) won = won || CheckVictory(new Vector2Int(position.x, position.y - 1), FlowDirection.Top);
                        if (attachedPipe.isOpenLeft) won = won || CheckVictory(new Vector2Int(position.x - 1, position.y), FlowDirection.Right);
                        if (attachedPipe.isOpenRight) won = won || CheckVictory(new Vector2Int(position.x + 1, position.y), FlowDirection.Left);

                        if (position == startPosition && attachedPipe.isOpenDown) won = true;
                        return won;
                    }
                    break;
                case FlowDirection.Left:
                    if (attachedPipe.isOpenLeft)
                    {
                        //pipeSprite.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.5f);
                        attachedPipe.hasCurrent = true;
                        //Dai corrente nelle altre direzioni
                        bool won = false;
                        //Dai corrente nelle altre direzioni
                        if (attachedPipe.isOpenDown)  won = won || CheckVictory(new Vector2Int(position.x, position.y - 1), FlowDirection.Top);
                        if (attachedPipe.isOpenUp)    won = won || CheckVictory(new Vector2Int(position.x, position.y + 1), FlowDirection.Bottom);
                        if (attachedPipe.isOpenRight) won = won || CheckVictory(new Vector2Int(position.x + 1, position.y), FlowDirection.Left);

                        if (position == startPosition && attachedPipe.isOpenDown) won = true ;
                        return won;
                    }
                    break;
                case FlowDirection.Right:
                    if (attachedPipe.isOpenRight)
                    {
                        //pipeSprite.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.5f);
                        attachedPipe.hasCurrent = true;
                        bool won = false;
                        //Dai corrente nelle altre direzioni
                        if (attachedPipe.isOpenDown) won = won || CheckVictory(new Vector2Int(position.x, position.y - 1), FlowDirection.Top);
                        if (attachedPipe.isOpenUp)   won = won || CheckVictory(new Vector2Int(position.x, position.y + 1), FlowDirection.Bottom);
                        if (attachedPipe.isOpenLeft) won = won || CheckVictory(new Vector2Int(position.x - 1, position.y), FlowDirection.Right);

                        if (position == startPosition && attachedPipe.isOpenDown) won = true;
                        return won;
                    }
                    break;
                case FlowDirection.Bottom:
                    if (attachedPipe.isOpenDown)
                    {
                        //pipeSprite.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.5f);
                        attachedPipe.hasCurrent = true;
                        //Dai corrente nelle altre direzioni
                        bool won = false;
                        //Dai corrente nelle altre direzioni
                        if (attachedPipe.isOpenRight) won = won || CheckVictory(new Vector2Int(position.x + 1, position.y), FlowDirection.Left);
                        if (attachedPipe.isOpenUp)   won = won || CheckVictory(new Vector2Int(position.x, position.y + 1), FlowDirection.Bottom);
                        if (attachedPipe.isOpenLeft) won = won || CheckVictory(new Vector2Int(position.x - 1, position.y), FlowDirection.Right);

                        return won;

                    }
                    break;
            }
        }
        return false;
    }

    public bool DoIWon()
    {
        clearPipes();
        if(CheckVictory(endPosition, FlowDirection.Top))
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j].GetComponent<GridCell>().canBeClicked = false;
                }
            }
            return true;
        }
        return false;
    }

    public int howMuchPipesAreUsed()
    {
        int pipeUsed = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject pipeSprite = grid[i, j].GetComponent<GridCell>().PipeSprite;
                if (pipeSprite != null)
                {
                    Pipe attachedPipe = pipeSprite.GetComponent<Pipe>();
                    if (attachedPipe.hasCurrent)
                    {
                        pipeUsed++;
                    }
                }
            }
        }
        return pipeUsed;
    }

    public void clearPipes()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject pipeSprite = grid[i, j].GetComponent<GridCell>().PipeSprite;
                if (pipeSprite != null)
                {
                    Pipe attachedPipe = pipeSprite.GetComponent<Pipe>();
                    pipeSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    attachedPipe.hasCurrent = false;
                }
            }
        }
    }
}
