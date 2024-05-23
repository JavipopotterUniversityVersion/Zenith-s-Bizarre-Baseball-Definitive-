using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomInstanceOnGridArea : MonoBehaviour, IBehaviour
{
    [SerializeField] Vector2Int _gridSize;
    [SerializeField] Vector2Int _gridOffset;
    [SerializeField] InstanceUnit[] _instances;
    Tilemap _tilemap;
    [SerializeField] Color _gizmosColor;

    private void OnDrawGizmos() {
        _tilemap = GetComponentInParent<Tilemap>();
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireCube(_tilemap.layoutGrid.WorldToCell(transform.position + (Vector3)(Vector3Int)_gridOffset), new Vector3(_gridSize.x, _gridSize.y, 0));
    }

    private void Awake() {
        _tilemap = GetComponentInParent<Tilemap>();
    }

    public void ExecuteBehaviour()
    {
        Vector3Int cellPosition = _tilemap.WorldToCell(transform.position + (Vector3)(Vector3Int)_gridOffset);
        Vector2 randomPosition = new Vector2(Random.Range(cellPosition.x - _gridSize.x, cellPosition.x + _gridSize.x) * _tilemap.layoutGrid.cellSize.x, Random.Range(cellPosition.y - _gridSize.y, cellPosition.y + _gridSize.y) * _tilemap.layoutGrid.cellSize.y);

        Instantiate(InstanceUnit.GetInstance(_instances), randomPosition, Quaternion.identity);
    }

    private void OnValidate() {
        name = "RandomInstanceOnGridArea";
    }
}
