using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSeter : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Vector2Int size;
    [SerializeField] RuleTile replacementTile;
    Vector3 tilePos;
    [SerializeField] float timeBetweenTiles = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 fixedPos = tilemap.CellToWorld(tilemap.WorldToCell(transform.position)) + new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0);
        Gizmos.DrawWireCube(fixedPos, new Vector3(size.x * tilemap.cellSize.x, size.y * tilemap.cellSize.y, 0));
    }

    private void Awake()
    {
        if(tilemap == null) tilemap = GetComponentInParent<Tilemap>();
    }

    public void SetTiles() => StartCoroutine(Set());

    public IEnumerator Set()
    {
        Vector3 initialPosition = transform.position - new Vector3(size.x * (tilemap.cellSize.x / 2) - tilemap.cellSize.x / 2, size.y * (tilemap.cellSize.y / 2) - tilemap.cellSize.x / 2, 0);
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                tilePos = initialPosition + new Vector3(i * tilemap.cellSize.x, j * tilemap.cellSize.y, 0);
                Vector3Int fixedTilePos = tilemap.WorldToCell(tilePos);
                tilemap.SetTile(fixedTilePos, replacementTile);
                yield return new WaitForSeconds(timeBetweenTiles);
            }
        }
    }
}