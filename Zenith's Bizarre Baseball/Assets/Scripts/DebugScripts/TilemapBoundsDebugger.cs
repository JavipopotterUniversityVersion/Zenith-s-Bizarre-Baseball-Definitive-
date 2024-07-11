using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBoundsDebugger : MonoBehaviour
{
    [SerializeField] Tilemap anotherTilemap;
    private void OnDrawGizmos() 
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        List<Vector3> positions = new List<Vector3>();
        if (tilemap == null) 
        {
            Debug.LogWarning("Tilemap component not found.");
            return;
        }

        foreach (var bounds in tilemap.cellBounds.allPositionsWithin) 
        {
            Vector3Int localPlace = new Vector3Int(bounds.x, bounds.y, bounds.z);
            Vector3 place = tilemap.CellToWorld(localPlace);
            if (tilemap.HasTile(localPlace)) 
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(place, Vector3.one);
                positions.Add(place);
            }
        }

        if (anotherTilemap == null) 
        {
            Debug.LogWarning("Another Tilemap component not found.");
            return;
        }

        foreach(Vector3 position in positions) 
        {
            Vector3Int cellPosition = anotherTilemap.WorldToCell(position);
            if (anotherTilemap.HasTile(cellPosition)) 
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(position, Vector3.one);
            }
        }
    }

    [ContextMenu("Paste")]
    public void Paste() 
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        if (tilemap == null) 
        {
            Debug.LogWarning("Tilemap component not found.");
            return;
        }

        foreach (var bounds in tilemap.cellBounds.allPositionsWithin) 
        {
            Vector3Int localPlace = new Vector3Int(bounds.x, bounds.y, bounds.z);
            Vector3 place = tilemap.CellToWorld(localPlace);
            if (tilemap.HasTile(localPlace)) 
            {
                anotherTilemap.SetTile(anotherTilemap.WorldToCell(place), tilemap.GetTile(localPlace));
            }
        }
    }
}
