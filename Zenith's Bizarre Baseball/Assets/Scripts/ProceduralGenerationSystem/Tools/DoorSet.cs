using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorSet : MonoBehaviour
{
    [HideInInspector] public Tilemap map;
    [HideInInspector] public Color ownColor;

    public void ColorItSelf() => map.color = ownColor;

    public void Initialize()
    {
        map = GetComponent<Tilemap>();
    }
    
    public void Set(Tilemap targetMap)
    {
        if (map == null) 
        {
            Debug.LogWarning("Tilemap component not found.");
            return;
        }

        foreach (var bounds in map.cellBounds.allPositionsWithin) 
        {
            Vector3Int localPlace = new Vector3Int(bounds.x, bounds.y, bounds.z);
            Vector3 place = map.CellToWorld(localPlace);
            if (map.HasTile(localPlace)) 
            {
                targetMap.SetTile(targetMap.WorldToCell(place), map.GetTile(localPlace));
            }
        }
    }
}
