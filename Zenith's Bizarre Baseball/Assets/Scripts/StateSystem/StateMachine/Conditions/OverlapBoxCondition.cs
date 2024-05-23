using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapBoxCondition : MonoBehaviour, ICondition
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 offset;

    [SerializeField] string _thingToDetectName = "something";

    [SerializeField] int minimunColliders = 1;
    [SerializeField] Color _gizmosColor;

    public bool CheckCondition()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + (Vector3)offset, size, 0, layerMask);
        return colliders.Length >= minimunColliders;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireCube(transform.position + (Vector3)offset, size);
    }

    private void OnValidate() {
        gameObject.name = $"At least {minimunColliders} of {_thingToDetectName}";
    }
}