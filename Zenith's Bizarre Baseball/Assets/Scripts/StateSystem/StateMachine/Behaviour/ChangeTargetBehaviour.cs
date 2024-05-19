using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTargetBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] Transform _targetTransform;
    public void ExecuteBehaviour()
    {
        if (_targetTransform != null) GetComponentInParent<TargetHandler>().SetTarget(_targetTransform);
        else GetComponentInParent<TargetHandler>().SetTarget(FindObjectOfType<InputManager>().transform);
    }

    void OnValidate()
    {
        if (_targetTransform != null) name = $"Change target to {_targetTransform.name}";
        else name = $"Change target to player";
    }
}
