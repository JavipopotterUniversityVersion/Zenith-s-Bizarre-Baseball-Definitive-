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
    [SerializeField] UnityEvent<Vector2> _onOverlapDirection = new UnityEvent<Vector2>();
    [SerializeField] bool debug = true;
 
    public bool CheckCondition()
    {
        if(overlapType == OverlapType.Box)
            return CheckBox();
        else
            return CheckCircle();
    }

    public void Overlap() => CheckCondition();

    private bool CheckBox()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + (Vector3)offset, size, 0, layerMask);

        if(colliders.Length > 0) 
        {
            _onOverlap.Invoke(colliders[0].transform);
            _onOverlapDirection.Invoke((colliders[0].transform.position - transform.position).normalized);
        }

        return colliders.Length >= minimunColliders;
    }

    private bool CheckCircle()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + (Vector3)offset, size.x, layerMask);

        if(colliders.Length > 0) 
        {
            _onOverlap.Invoke(colliders[0].transform);
            _onOverlapDirection.Invoke((colliders[0].transform.position - transform.position).normalized);
        }

        return colliders.Length >= minimunColliders;
    }

    private void OnDrawGizmos()
    {
        if(!debug) return;
        Gizmos.color = _gizmosColor;
        
        if(overlapType == OverlapType.Box)
            Gizmos.DrawWireCube(transform.position + (Vector3)offset, size);
        else
            Gizmos.DrawWireSphere(transform.position + (Vector3)offset, size.x);
    }

    private void OnValidate() {
        gameObject.name = $"At least {minimunColliders} of {_thingToDetectName}";
    }

    public void SetTargetLayer(int layerMask) => this.layerMask.value = layerMask;
    public void SetTargetLayer(string layerName) => layerMask.value = LayerMask.GetMask(layerName);
}