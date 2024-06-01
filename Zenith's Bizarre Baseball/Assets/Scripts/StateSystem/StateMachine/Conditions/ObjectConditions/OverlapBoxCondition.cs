using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverlapBoxCondition : MonoBehaviour, ICondition
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 offset;

    [SerializeField] string _thingToDetectName = "something";

    [SerializeField] int minimunColliders = 1;
    [SerializeField] Color _gizmosColor = Color.red;

    enum OverlapType { Box, Circle}
    [SerializeField] OverlapType overlapType;

    [SerializeField] UnityEvent<Transform> _onOverlap = new UnityEvent<Transform>();

    public bool CheckCondition()
    {
        if(overlapType == OverlapType.Box)
            return CheckBox();
        else
            return CheckCircle();
    }

    private bool CheckBox()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + (Vector3)offset, size, 0, layerMask);
        OnOverLap(colliders);
        return colliders.Length >= minimunColliders;
    }

    private bool CheckCircle()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + (Vector3)offset, size.x, layerMask);
        OnOverLap(colliders);
        return colliders.Length >= minimunColliders;
    }

    void OnOverLap(Collider2D[] colliders)
    {
        if(colliders.Length > 0) _onOverlap.Invoke(colliders[0].transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        
        if(overlapType == OverlapType.Box)
            Gizmos.DrawWireCube(transform.position + (Vector3)offset, size);
        else
            Gizmos.DrawWireSphere(transform.position + (Vector3)offset, size.x);
    }

    private void OnValidate() {
        gameObject.name = $"At least {minimunColliders} of {_thingToDetectName}";
    }

    public void SetTargetLayer(int layerMask) => this.layerMask = layerMask;
}