using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerAnimation : MonoBehaviour
{
    Pointer pointer;
    Animator animator;

    private void Awake()
    {
        pointer = GetComponent<Pointer>();
        animator = GetComponentInParent<Animator>();
        pointer.OnPoint.AddListener(AnimatePointer);
    }

    void AnimatePointer(Vector2 direction)
    {
        animator.SetFloat("PX", direction.x);
        animator.SetFloat("PY", direction.y);
    }
}
