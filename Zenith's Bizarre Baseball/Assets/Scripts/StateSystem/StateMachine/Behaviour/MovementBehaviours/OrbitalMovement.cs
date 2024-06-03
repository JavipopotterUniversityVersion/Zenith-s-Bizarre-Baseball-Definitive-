using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalMovement : MonoBehaviour, IBehaviour
{
    MovementController movementController;
    TargetHandler targetHandler;
    [SerializeField] float orbit_radius = 5f;

    [Range(0, 1)]
    [SerializeField] float adjustFactor = 0.5f;

    [Range(0, 1)]
    [SerializeField] float weight = 1f;

    [SerializeField] float multiplier = 1f;

    private void Awake() {
        movementController = GetComponentInParent<MovementController>();
        targetHandler = GetComponentInParent<TargetHandler>();
    }

    public void ExecuteBehaviour()
    {
        //Describe an orbit around the target
        Vector2 direction = (targetHandler.Target.position - transform.position).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x).normalized;
        Vector2 finalDirection;

        if(Vector2.Distance(targetHandler.Target.position, transform.position) < orbit_radius + 0.5f)
            finalDirection = Vector2.Lerp(perpendicular, -direction, Time.deltaTime * adjustFactor * 100).normalized;
        else if(Vector2.Distance(targetHandler.Target.position, transform.position) > orbit_radius - 0.5f)
            finalDirection = Vector2.Lerp(perpendicular, direction, Time.deltaTime * adjustFactor * 100).normalized;
        else
            finalDirection = perpendicular;

        Vector2 ultimateDirection = Vector2.Lerp(movementController.Rb.velocity.normalized, finalDirection, Time.deltaTime * weight * 100) * multiplier;
        movementController.Move(ultimateDirection);
    }

    private void OnValidate() {
        movementController = GetComponentInParent<MovementController>();
        if(movementController == null) return;

        name = $"Orbit: { 1/(2 * Mathf.PI / (movementController.Speed * multiplier / orbit_radius))} r/s";
    }
}
