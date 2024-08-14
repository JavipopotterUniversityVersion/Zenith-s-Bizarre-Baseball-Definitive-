using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalMovementByTransform : MonoBehaviour, IBehaviour
{
    [SerializeField] float _radius;
    [SerializeField] float _orbitsPerSecond;
    TargetHandler _targetHandler;

    [SerializeField] bool _faceOutwards;

    [SerializeField] Transform _orbiter;

    private void Awake() 
    {
        _targetHandler = GetComponentInParent<TargetHandler>();
    }

    public void ExecuteBehaviour()
    {
        _orbiter.position = _targetHandler.Target.transform.position + new Vector3(Mathf.Cos(Time.time * _orbitsPerSecond * 8) * _radius, Mathf.Sin(Time.time * _orbitsPerSecond * 8) * _radius, 0);
        if (_faceOutwards) _orbiter.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(_orbiter.position.y - _targetHandler.Target.transform.position.y, _orbiter.position.x - _targetHandler.Target.transform.position.x) * Mathf.Rad2Deg - 90);
    }

    private void OnValidate() {
        name = $"Orbit {_radius} radius & {_orbitsPerSecond} ops";
    }
}
