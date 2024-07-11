using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFixedPositionOnAwake : MonoBehaviour
{
    public List<Vector2Int> posiblePositions = new List<Vector2Int>();

    private void Awake()
    {
        posiblePositions.Add(Vector2Int.zero);
        Vector2Int selectedPosition = posiblePositions[Random.Range(0, posiblePositions.Count)];
        transform.position += new Vector3(selectedPosition.x, selectedPosition.y, 0) * 2;
    }

    private void OnDrawGizmos()
    {
        foreach(Vector2Int position in posiblePositions)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + new Vector3(position.x, position.y, 0) * 2, Vector3.one);
        }
    }
}
