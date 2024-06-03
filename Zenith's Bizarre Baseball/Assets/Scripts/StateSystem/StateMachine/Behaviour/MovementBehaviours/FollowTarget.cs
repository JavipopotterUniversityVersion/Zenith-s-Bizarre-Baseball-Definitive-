using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour, IBehaviour
{
    TargetHandler targetHandler;
    MovementController movementController;
    [SerializeField]float multiplier = 1;
    [Range(0, 1)]
    [SerializeField] float weight = 1f;

    private void Awake()
    { 
        targetHandler = GetComponentInParent<TargetHandler>();
        movementController = GetComponentInParent<MovementController>();
    }

    public void ExecuteBehaviour()
    {
        Vector2 direction = targetHandler.GetTargetDirection() * multiplier;
        Vector2 fixedDirection = Vector2.Lerp(movementController.Rb.velocity.normalized, direction, Time.deltaTime * weight * 100);
        movementController.Move(fixedDirection);
    }

    private void OnValidate() {
        movementController = GetComponentInParent<MovementController>();
        if(movementController == null) return;

        if(multiplier >= 1)
            gameObject.name = $"Follow Target {multiplier * movementController.Speed} m/s";
        else
            gameObject.name = $"Move Away {multiplier * movementController.Speed} m/s";
    }
}
