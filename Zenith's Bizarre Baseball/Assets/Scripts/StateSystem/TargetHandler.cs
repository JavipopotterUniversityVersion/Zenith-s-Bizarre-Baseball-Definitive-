using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TargetHandler : MonoBehaviour
{
    [SerializeField] Transform _target;
    public Transform target => _target;

    private void OnEnable() {
        if (_target == null)
            _target = FindAnyObjectByType<InputManager>().transform;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
