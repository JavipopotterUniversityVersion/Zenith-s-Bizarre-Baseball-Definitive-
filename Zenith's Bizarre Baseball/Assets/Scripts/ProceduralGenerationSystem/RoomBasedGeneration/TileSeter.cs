using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSeter : MonoBehaviour
{
    Tilemap tilemap;
    Grid grid;
    [SerializeField] Vector2Int size;
    [SerializeField] Tile replacementTile;
    [Range(0, 1)]
    [SerializeField] float probability;

    private void OnDrawGizmos()
    {
        //grid = GetComponentInParent<Grid>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0));
    }

    private void Awake()
    {
        tilemap = GetComponentInParent<Tilemap>();
        grid = GetComponentInParent<Grid>();
    }

    public void SetTiles()
    {
        Vector3 initialPosition = transform.position - new Vector3(size.x * grid.cellSize.x / 2, size.y * grid.cellSize.y / 2, 0);
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3 tilePos = initialPosition + new Vector3(i * grid.cellSize.x, j * grid.cellSize.y, 0);
                Vector3Int fixedTilePos = tilemap.WorldToCell(tilePos);
                tilemap.SetTile(fixedTilePos, replacementTile);
            }
        }
    }
}