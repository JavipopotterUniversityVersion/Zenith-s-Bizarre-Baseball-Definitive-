using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToTargetBehaviour : MonoBehaviour, IBehaviour
{
    TargetHandler targetHandler;
    [SerializeField] Pointer pointer;
    [SerializeField] float rotationSpeed = 5f;

    private void Awake() {
        targetHandler = GetComponentInParent<TargetHandler>();
    }

    public void SetRotationSpeed(float newSpeed) => rotationSpeed = newSpeed;

    public void ExecuteBehaviour() {
        Vector2 direction = Vector2.Lerp(transform.up, (targetHandler.target.position - transform.position).normalized, Time.deltaTime * rotationSpeed);
        pointer.SetPointer(direction);
    }

    private void OnValidate() {
        name = $"Look Target: {rotationSpeed} rad/s";
    }
}
