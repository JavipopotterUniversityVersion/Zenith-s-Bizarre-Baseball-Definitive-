using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TargetHandler : MonoBehaviour
{
    [SerializeField] Transform _target;
    public Transform Target => _target;

    private void OnEnable() {
        SetTarget(_target);
    }

    public void SetTarget(Transform target)
    {
        if (target == null)
            _target = FindAnyObjectByType<InputManager>().transform;

        _target = target;
    }

    public Vector2 GetTargetDirection()
    {
        return (Target.position - transform.position).normalized;
    }
}
