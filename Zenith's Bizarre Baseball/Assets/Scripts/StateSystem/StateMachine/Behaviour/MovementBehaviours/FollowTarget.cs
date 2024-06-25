using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour, IBehaviour
{
    TargetHandler targetHandler;
    MovementController movementController;
    [SerializeField] float multiplier = 1;
    [SerializeField] ObjectProcessor _multiplierProcessor;
    [Range(0, 1)]
    [SerializeField] float weight = 1f;
    [SerializeField] bool instant = false;

    private void Awake()
    { 
        targetHandler = GetComponentInParent<TargetHandler>();
        movementController = GetComponentInParent<MovementController>();
    }

    public void ExecuteBehaviour()
    {
        Vector2 direction = targetHandler.GetTargetDirection();
        Vector2 fixedDirection;
        
        if(instant) fixedDirection = direction;
        else fixedDirection = Vector2.Lerp(movementController.Rb.velocity.normalized, direction, Time.deltaTime * weight * 100).normalized;

        movementController.Move(fixedDirection * _multiplierProcessor.Result(multiplier));
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
