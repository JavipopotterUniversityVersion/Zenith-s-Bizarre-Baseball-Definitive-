using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class DoorSet : MonoBehaviour
{
    [HideInInspector] public Tilemap map;
    [HideInInspector] Color _ownColor;
    [SerializeField] Gradient gradient;
    [SerializeField] UnityEvent onSet = new UnityEvent();
    [SerializeField] UnityEvent<Color> onColorItSelf = new UnityEvent<Color>();
    [SerializeField] TileBase nullTile;

    public void ColorItSelf()
    {
        map.color = _ownColor;
        onColorItSelf?.Invoke(_ownColor);
    }

    public void ResetColor()
    {
        map.color = Color.white;
        onColorItSelf?.Invoke(Color.white);
    }

    public void Initialize()
    {
        _ownColor = gradient.Evaluate(Random.Range(0, 1f));
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
                TileBase tile = map.GetTile(localPlace);
                if (tile == nullTile) tile = null;

                targetMap.SetTile(targetMap.WorldToCell(place), tile);
            }
        }

        GetComponentInParent<TilemapSetsContainer>().Sets.Add(map);

        onSet?.Invoke();
    }
}
