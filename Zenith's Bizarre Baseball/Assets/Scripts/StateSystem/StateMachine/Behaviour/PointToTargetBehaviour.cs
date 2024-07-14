using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToTargetBehaviour : MonoBehaviour, IBehaviour
{
    TargetHandler targetHandler;
    [SerializeField] Pointer pointer;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] ObjectProcessor _offset;
    [SerializeField] bool instant;

    private void Awake() {
        targetHandler = GetComponentInParent<TargetHandler>();
    }

    public void SetRotationSpeed(float newSpeed) => rotationSpeed = newSpeed;

    public void ExecuteBehaviour() {
        Vector2 targetDirection = (targetHandler.Target.position - transform.position).normalized;
        Vector2 perpendicular = new Vector2(-targetDirection.y, targetDirection.x);

        Vector2 finalDirection = targetDirection + perpendicular * _offset.Result(0);

        Vector2 direction = instant ? Vector2.Lerp(transform.up, finalDirection, Time.deltaTime * rotationSpeed) : finalDirection;
        pointer.SetPointer(direction);
    }

    private void OnValidate() {
        if(instant) name = $"Look Target: Instant";
        else name = $"Look Target: {rotationSpeed} rad/s";
    }
}
