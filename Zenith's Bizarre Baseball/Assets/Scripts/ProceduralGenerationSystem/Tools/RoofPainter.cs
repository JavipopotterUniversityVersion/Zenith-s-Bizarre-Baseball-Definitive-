using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoofPainter : MonoBehaviour
{
    Tilemap _tilemapToPaint;
    [SerializeField] Tilemap _tilemapToCopyFrom;
    [SerializeField] TileBase _roofTile;
    RoomNode _node;

    private void Start() 
    {
        _node = GetComponentInParent<RoomNode>();
        _tilemapToPaint = GetComponent<Tilemap>();
        _node.Generator.OnFinishedGeneration.AddListener(() => StartCoroutine(Paint()));
    }

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
    }
}
