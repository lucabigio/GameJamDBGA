using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPosition
{
    private Vector2Int _position;
    public List<Vector2Int> Neighbours { get; private set; }
    public MapPosition(Vector2Int pos, Vector2Int topLeftCorner, Vector2Int bottomRightCorner)
    {
        _position = pos;
        Neighbours = new List<Vector2Int>();    
        CalculateNeighbours(topLeftCorner, bottomRightCorner);
    }
    private void CalculateNeighbours(Vector2Int topLeftCorner, Vector2Int bottomRightCorner)
    {
        AddIfPossibleNeighbour(Vector2Int.left, topLeftCorner, bottomRightCorner);
        AddIfPossibleNeighbour(Vector2Int.up, topLeftCorner, bottomRightCorner);
        AddIfPossibleNeighbour(Vector2Int.down, topLeftCorner, bottomRightCorner);
        AddIfPossibleNeighbour(Vector2Int.right, topLeftCorner, bottomRightCorner);
    }

    private void AddIfPossibleNeighbour(Vector2Int direction, Vector2Int topLeftCorner, Vector2Int bottomRightCorner)
    {
        Vector2Int leftPos = _position + direction;
        if (leftPos.x >= topLeftCorner.x &&
            leftPos.y >= topLeftCorner.y &&
            leftPos.x <= bottomRightCorner.x &&
            leftPos.y <= bottomRightCorner.y) Neighbours.Add(leftPos);
    }
}
