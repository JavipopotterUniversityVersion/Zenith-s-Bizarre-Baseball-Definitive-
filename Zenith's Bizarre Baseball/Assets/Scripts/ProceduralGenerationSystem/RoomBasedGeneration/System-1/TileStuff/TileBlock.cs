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

    public void SetTile(Tilemap targeTileMap)
    {
        TileBase tile = thisMap.GetTile(thisMap.WorldToCell(transform.position));
        targeTileMap.SetTile(targeTileMap.WorldToCell(transform.position), tile);
    }
}
