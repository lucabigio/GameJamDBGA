using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public static PathController Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        CreatePath(new Vector2Int(1,1), new Vector2Int(4, 5));
    }


    //For now it creates a path choosing a completely random direction of what is available
    void CreatePath(Vector2Int startPos, Vector2Int endPos)
    {
        Vector2Int topLeftCorner = Vector2Int.zero;
        Vector2Int bottomRightCorner = new Vector2Int(6, 6);
        //TODO: Sostituire con il MapController quando verrà implementato
        //Vector2Int bottomRightCorner = new Vector2Int(MapController.Instance.map.width, MapController.Instance.map.height);
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int currentPathPos = startPos;
        path.Add(startPos);
        while (currentPathPos != endPos)
        {
            MapPosition temporaryPos = new MapPosition(currentPathPos, topLeftCorner, bottomRightCorner);
            Vector2Int newPos = Vector2Int.zero;
            //Choosing a random direction until it is not in path;
            
            newPos = temporaryPos.Neighbours[Random.Range(0, temporaryPos.Neighbours.Count - 1)];


            path.Add(newPos);
            currentPathPos = newPos;            
        }
        Debug.Log("PERCORSO SCELTO PARTENDO DA: " + startPos + " E ARRIVANDO A "+ endPos);
        foreach (Vector2Int elem in path){
            Debug.Log(elem);
        }

    }


}
