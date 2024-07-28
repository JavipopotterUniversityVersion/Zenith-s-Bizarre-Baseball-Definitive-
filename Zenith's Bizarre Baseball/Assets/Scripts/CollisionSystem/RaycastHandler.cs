using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    [SerializeField] CollidableGroup[] OnEnterCollidables;
    public CollidableGroup[] OnEnterCollidablesProp() => OnEnterCollidables;

    [SerializeField] CollidableGroup[] OnStayCollidables;
    public CollidableGroup[] OnStayCollidablesProp() => OnStayCollidables;

    [SerializeField] CollidableGroup[] OnExitCollidables;
    public CollidableGroup[] OnExitCollidablesProp() => OnExitCollidables;
    
    [SerializeField] bool _drawing;
    public bool Drawing => _drawing;

    [SerializeField] bool _checking;
    public bool Checking => _checking;

    public void SetChecking(bool value)
    {
        _checking = value;
        if(value) _drawing = false;
    }

    public void SetDrawing(bool value) => _drawing = value;

    [SerializeField] float raycastDistance = Mathf.Infinity;
    RaycastHit2D _lastHit;
    public Vector3 LastHitPoint => _lastHit.point;

    [SerializeField] LayerMask _targetLayers;

    private void FixedUpdate() 
    {
        if(_drawing) _lastHit = Physics2D.Raycast(transform.position, transform.up, raycastDistance, _targetLayers);

        if(_checking)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, raycastDistance, _targetLayers);

            if(hit.collider != null)
            {
                if(_lastHit.collider != hit.collider)
                {
                    CollidableGroup.CheckCollidables(OnExitCollidables, _lastHit.collider);
                    CollidableGroup.CheckCollidables(OnEnterCollidables, hit.collider);
                }

                CollidableGroup.CheckCollidables(OnStayCollidables, hit.collider);
            }

            _lastHit = hit;
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * raycastDistance);
    }
}
