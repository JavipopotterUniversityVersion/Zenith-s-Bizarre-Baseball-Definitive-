using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinderMap : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] List<TileBase> walkableTiles;

    readonly Vector3Int[] directions = new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

    public List<Vector2> Path(Vector2 start, Vector2 end)
    {
        List<Vector2> path = new List<Vector2>();

        Vector3Int startCell = tilemap.WorldToCell(start);
        Vector3Int endCell = tilemap.WorldToCell(end);

        List<Vector3Int> positionsToCheck = new List<Vector3Int>();
        List<Vector3Int> checkedPositions = new List<Vector3Int>();
        
        List<List<Vector3Int>> paths = new List<List<Vector3Int>>();
        positionsToCheck.Add(startCell);

        while(positionsToCheck.Count > 0)
        {
            List<Vector3Int> newPositionsToCheck = new List<Vector3Int>();
            foreach (Vector3Int position in positionsToCheck)
            {
                List<Vector3Int> newPath = new List<Vector3Int>();
            }
        }

        return path;
    }

    private List<Vector3Int> GetValidNeighbours(Vector3Int position, List<Vector3Int> checkedPositions)
    {
        List<Vector3Int> neighbours = new List<Vector3Int>();
        foreach (Vector3Int direction in directions)
        {
            if (IsWalkable(position + direction) && !checkedPositions.Contains(position + direction))
            {
                neighbours.Add(position + direction);
            }
        }
        return neighbours;
    }

    bool IsWalkable(Vector3Int position)
    {
        TileBase tile = tilemap.GetTile(position);
        return walkableTiles.Contains(tile);
    }
}
