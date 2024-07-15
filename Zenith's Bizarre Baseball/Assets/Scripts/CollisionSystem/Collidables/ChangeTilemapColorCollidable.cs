using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeTilemapColorCollidable : ICollidable
{
    [SerializeField] Tilemap _targetTilemap;    
    [SerializeField] Color _color;

    private void Awake() 
    {
        if(_targetTilemap == null) _targetTilemap = GetComponent<Tilemap>();
    }

    public override void OnCollide(Collider2D collider)
    {
        _targetTilemap.color = _color;
    }
}
