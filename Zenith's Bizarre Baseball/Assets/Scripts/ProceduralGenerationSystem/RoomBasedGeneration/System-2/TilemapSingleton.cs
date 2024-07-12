using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapSingleton : MonoBehaviour
{
    public static TilemapSingleton Instance { get; private set; }

    public Tilemap map;

    private void Awake()
    {
        map = GetComponent<Tilemap>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PasteOnMap(Tilemap otherMap)
    {
        foreach (var bounds in otherMap.cellBounds.allPositionsWithin) 
        {
            Vector3Int localPlace = new Vector3Int(bounds.x, bounds.y, bounds.z);
            Vector3 place = otherMap.CellToWorld(localPlace);
            if (otherMap.HasTile(localPlace)) 
            {
                map.SetTile(map.WorldToCell(place), otherMap.GetTile(localPlace));
            }
        }
    }
}
