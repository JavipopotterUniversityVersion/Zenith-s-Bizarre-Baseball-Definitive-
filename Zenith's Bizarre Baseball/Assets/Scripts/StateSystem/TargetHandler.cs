using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TargetHandler : MonoBehaviour
{
    [SerializeField] Transform _target;
    public Transform Target
    {
        get 
        {
            if(_target == null) _target = FindAnyObjectByType<InputManager>().transform;
            return _target;
        }
        private set
        {
            if(value == null) _target = FindAnyObjectByType<InputManager>().transform;
            else _target = value;
        }
    }

    private void Awake()
    {
        if(_target == null) _target = FindFirstObjectByType<InputManager>().transform;
    }

    private void OnEnable() => Awake();

    public void SetTarget(Transform target) => Target = target;
    public Transform GetTarget() => Target;

    public Vector2 GetTargetDirection()
    {
        return (Target.position - transform.position).normalized;
    }
}
