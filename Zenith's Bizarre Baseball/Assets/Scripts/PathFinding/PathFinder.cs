using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathFinder : MonoBehaviour, ICondition, IBehaviour
{
    PathFindingMap map;
    Transform target;
    Transform finalTarget;
    [SerializeField] UnityEvent<Vector2> _onGiveDirection;
    LayerMask targetLayers;
    [SerializeField] Rect rect;

    private void Awake() {
        map = GetComponentInParent<PathFindingMap>();
    }

    private void OnDrawGizmos() {
        Rect realRect = rect;
        realRect.position += (Vector2)transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(realRect.position, realRect.size);
    }

    private void Start() {
        targetLayers =  LayerMask.GetMask("Walls") | LayerMask.GetMask("Player"); 
        finalTarget = GetComponentInParent<TargetHandler>().Target;
    }

    public bool CheckCondition()
    {
        return target == finalTarget;
    }

    public void ExecuteBehaviour() {
        Rect realRect = rect;
        realRect.position += (Vector2)transform.position;

        if(target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            _onGiveDirection.Invoke(direction);
        
            RaycastHit2D hit = Physics2D.BoxCast(realRect.position, realRect.size, 0, finalTarget.position - transform.position, Mathf.Infinity, targetLayers);
            if(hit && hit.transform == finalTarget) target = finalTarget;
            else target = map.FindPath(transform, realRect);
        }
        else
        {
            target = map.FindPath(transform, realRect);
        }
    }
}
