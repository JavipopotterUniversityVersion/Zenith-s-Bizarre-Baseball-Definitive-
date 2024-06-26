using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] GameObject _targetObject;

    private void Awake() 
    {
        if(_targetObject == null)   _targetObject = GetComponentInParent<StateHandler>().transform.parent.gameObject;
    }

    public void ExecuteBehaviour()
    {
        Destroy(_targetObject);
    }

    private void OnValidate()
    {
        name = "Destroy this gameobject";
    }
}
