using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    MovementController movementController;
    Animator animator;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        animator = GetComponent<Animator>();
        movementController.OnMoving.AddListener(AnimateMovement);
    }

    public void AnimateMovement(Vector2 direction)
    {
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }
}
