using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class RandomInstanceOnGridArea : MonoBehaviour, IBehaviour
{
    [SerializeField] Vector2Int _size;
    [SerializeField] Vector2Int offset;
    [SerializeField] InstanceUnit[] _instances;
    [SerializeField] Color _gizmosColor;


    private void OnDrawGizmos() {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireCube(transform.position + (Vector3Int) offset, new Vector3(_size.x * 2, _size.y * 2, 0));

    }

    public void ExecuteBehaviour()
    {
        Vector2Int randomPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y) + 
        new Vector2Int(Random.Range(-_size.x, _size.x), Random.Range(-_size.y, _size.y)) + offset;

        Instantiate(InstanceUnit.GetRandomInstance(_instances), (Vector3Int) randomPosition, Quaternion.identity);
    }

    private void OnValidate() {
        name = "RandomInstanceOnGridArea";
    }
}
