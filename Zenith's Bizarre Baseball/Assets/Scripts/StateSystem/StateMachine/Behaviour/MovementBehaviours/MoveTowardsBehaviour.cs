using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] Transform _target;
    MovementController _movementController;
    [SerializeField] float multiplier = 1;

    private void Start()
    {
        _movementController = GetComponentInParent<MovementController>();
    }

    public void ExecuteBehaviour()
    {
        Vector2 direction = (_target.position - transform.position).normalized;
        _movementController.Move(direction * multiplier);
    }

    private void OnValidate() {
        _movementController = GetComponentInParent<MovementController>();
        if(_target == null || _movementController == null) return;
        Vector2 direction = (_target.position - transform.position).normalized;
        name = $"Move Towards {direction} at {multiplier * _movementController.Speed} m/s";
    }
}
