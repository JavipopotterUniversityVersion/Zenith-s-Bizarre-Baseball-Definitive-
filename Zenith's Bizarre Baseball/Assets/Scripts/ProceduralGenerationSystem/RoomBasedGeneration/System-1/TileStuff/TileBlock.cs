using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBlock : MonoBehaviour
{
    Tilemap thisMap;

    private void Awake()
    {
        thisMap = GetComponentInParent<Tilemap>();
    }

    public void SetTile(Tilemap targetTileMap)
    {
        TileBase tile = thisMap.GetTile(thisMap.WorldToCell(transform.position));
        targetTileMap.SetTile(targetTileMap.WorldToCell(transform.position), tile);
    }
}
