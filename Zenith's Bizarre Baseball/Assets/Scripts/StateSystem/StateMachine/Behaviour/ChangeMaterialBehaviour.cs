using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] SpriteRenderer _targetSr;
    [SerializeField] Material _material;

    public void ExecuteBehaviour() => _targetSr.material = _material;

    private void OnValidate() 
    {
        if(_targetSr != null && _material != null)
        {
            name = $"Change { _targetSr.name } material to { _material.name}";
        }
    }
}
