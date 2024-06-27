using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParentBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] Transform _targetChild;
    [SerializeField] Transform _targetParent;

    public void ExecuteBehaviour()
    {
        _targetChild.SetParent(_targetParent);
    }

    private void OnValidate() 
    {
        string childName = _targetChild == null ? "null" : _targetChild.name;
        string parentName = _targetParent == null ? "null" : _targetParent.name;

        name = $"Set {childName} as {parentName} child";
    }
}
