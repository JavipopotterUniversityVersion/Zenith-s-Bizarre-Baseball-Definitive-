using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackToOriginalPositionBehaviour : MonoBehaviour, IBehaviour, ICondition
{
    [SerializeField] float _maxDistance = 3;
    private Vector3 _originalPosition;
    MovementController _movementController;
    bool returning = false;
    const float MARGIN = 0.1f;

    private void Awake() {
        _originalPosition = transform.position;
        _movementController = GetComponentInParent<MovementController>();
    }

    public bool CheckCondition()
    {
        if(returning) return true;
        return Vector3.Distance(transform.position, _originalPosition) > _maxDistance;
    }

    public void ExecuteBehaviour()
    {
        if(Vector3.Distance(transform.position, _originalPosition) > _maxDistance) returning = true;

        if(returning)
        {
            if(Vector3.Distance(transform.position, _originalPosition) > MARGIN) _movementController.MoveTo(_originalPosition);
            else returning = false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _maxDistance);
    }
}
