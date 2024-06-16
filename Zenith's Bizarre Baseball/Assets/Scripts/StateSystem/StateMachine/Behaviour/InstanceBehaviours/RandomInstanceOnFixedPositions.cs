using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInstanceOnFixedPositions : MonoBehaviour, IBehaviour
{
    [SerializeField] InstanceUnit[] _instances;
    [SerializeField] Vector2[] _positions;

    public void ExecuteBehaviour()
    {
        foreach (Vector2 position in _positions)
        {
            Instantiate(InstanceUnit.GetInstance(_instances), position + (Vector2)transform.position, Quaternion.identity);
        }
    }

    private void OnValidate() {
        name = "RandomInstanceOnFixedPositions";
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        foreach (Vector2 position in _positions)
        {
            Gizmos.DrawWireCube(position + (Vector2)transform.position, Vector3.one);
        }
    }
}
