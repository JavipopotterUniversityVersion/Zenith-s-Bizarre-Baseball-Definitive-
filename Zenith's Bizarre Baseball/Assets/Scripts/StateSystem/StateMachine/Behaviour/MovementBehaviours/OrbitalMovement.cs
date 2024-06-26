using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class OrbitalMovement : MonoBehaviour, IBehaviour
{
    MovementController movementController;
    TargetHandler targetHandler;
    [SerializeField] float orbit_radius = 5f;
    [SerializeField] ObjectProcessor _radiusProcessor;

    [Range(0, 100)]
    [SerializeField] float adjustFactor = 0.5f;

    [Range(0, 100)]
    [SerializeField] float weight = 1f;

    [SerializeField] float multiplier = 1f;
    [SerializeField] ObjectProcessor _multiplierProcessor;

    private void Awake() {
        movementController = GetComponentInParent<MovementController>();
        targetHandler = GetComponentInParent<TargetHandler>();
    }

    public void SetOrbitRadius(float radius)
    {
        orbit_radius = radius;
    }

    public void SetMultiplier(float multiplier)
    {
        this.multiplier = multiplier;
    }

    public void MultiplyMultiplier(float multiplier)
    {
        this.multiplier *= multiplier;
    }

    public void ExecuteBehaviour()
    {
        Vector2 direction = (targetHandler.Target.position - transform.position).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x).normalized * Mathf.Sign(_multiplierProcessor.Result(multiplier));
        Vector2 finalDirection;

        if(Vector2.Distance(targetHandler.Target.position, transform.position) < _radiusProcessor.Result(orbit_radius) + 0.5f)
            finalDirection = Vector2.Lerp(perpendicular, -direction, Time.deltaTime * adjustFactor * 100).normalized;
        else if(Vector2.Distance(targetHandler.Target.position, transform.position) > _radiusProcessor.Result(orbit_radius) - 0.5f)
            finalDirection = Vector2.Lerp(perpendicular, direction, Time.deltaTime * adjustFactor * 100).normalized;
        else
            finalDirection = perpendicular;

        Vector2 ultimateDirection = Vector2.Lerp(movementController.Rb.velocity.normalized, 
        finalDirection, Time.deltaTime * weight * 100) * Mathf.Abs(_multiplierProcessor.Result(multiplier));

        movementController.Move(ultimateDirection);
    }

    private void OnValidate() {
        movementController = GetComponentInParent<MovementController>();
        if(movementController == null) return;

        name = $"Orbit: { 1/(2 * Mathf.PI / (movementController.Speed * multiplier / orbit_radius))} r/s";
    }
}
