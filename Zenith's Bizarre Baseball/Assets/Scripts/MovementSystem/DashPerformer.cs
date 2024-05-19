using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPerformer : MonoBehaviour
{
    MovementController movementController;
    Rigidbody2D rb;
    Vector2 dashDirection;

    [SerializeField] float dashSpeed = 20f;

    private void Start() {
        movementController = GetComponent<MovementController>();
        rb = movementController.Rb;
        movementController.OnMoving.AddListener((Vector2) => dashDirection = Vector2);
    }

    public void Dash()
    {
        rb.velocity = dashDirection * dashSpeed;
    }
}
