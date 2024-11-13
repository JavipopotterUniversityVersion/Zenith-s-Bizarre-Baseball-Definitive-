using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoofPainter : MonoBehaviour
{
    Tilemap _tilemapToPaint;
    [SerializeField] Tilemap _tilemapToCopyFrom;
    [SerializeField] TileBase _roofTile;
    [SerializeField] TileBase _doorTile;
    RoomNode _node;
    [SerializeField] bool paintOnStart = true;
    [SerializeField] bool paintDoors = true;

    private void Start() 
    {
        _node = GetComponentInParent<RoomNode>();
        _tilemapToPaint = GetComponent<Tilemap>();
        if(paintOnStart) _node.Generator.OnFinishedGeneration.AddListener(() => StartCoroutine(Paint()));
    }

    public void PaintRoof() => StartCoroutine(Paint());

    public IEnumerator Paint()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (var bounds in _tilemapToCopyFrom.cellBounds.allPositionsWithin) 
        {
            Vector3Int localPlace = new Vector3Int(bounds.x, bounds.y, bounds.z);
            Vector3 place = _tilemapToCopyFrom.CellToWorld(localPlace);

            if (_tilemapToCopyFrom.HasTile(localPlace)) 
            {
                _tilemapToPaint.SetTile(_tilemapToPaint.WorldToCell(place), _roofTile);
            }
        }

        List<Tilemap> sets = GetComponentInParent<TilemapSetsContainer>().Sets;
        if(!paintDoors) yield break;

        foreach (var set in sets)
        {
            foreach (var bounds in set.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(bounds.x, bounds.y, bounds.z);
                Vector3 place = set.CellToWorld(localPlace);

                if (set.HasTile(localPlace))
                {
                    _tilemapToPaint.SetTile(_tilemapToPaint.WorldToCell(place), _doorTile);
                }
            }
        }
    }

    bool HasAnyNullAdjacentTile(Vector3Int pos, Tilemap map)
    {
        if(map.GetTile(pos + Vector3Int.up) == null) return true;
        if(map.GetTile(pos + Vector3Int.down) == null) return true;
        if(map.GetTile(pos + Vector3Int.left) == null) return true;
        if(map.GetTile(pos + Vector3Int.right) == null) return true;

        return false;
    }
}
