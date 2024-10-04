using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReplaceProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] InstanceUnit[] _possibleReplacements;
    [SerializeField] UnityEvent<GameObject> onReplacedObject;
    [SerializeField] bool _keepRotation = false;

    public void Process(GameObject gameObject)
    {
        if(gameObject == null) return;
        
        Quaternion rotation = _keepRotation ? gameObject.transform.rotation : Quaternion.identity;

        GameObject replacementObject = InstanceUnit.GetRandomInstance(_possibleReplacements);
        Instantiate(replacementObject, gameObject.transform.position, rotation);
        onReplacedObject.Invoke(gameObject);

        Destroy(gameObject);
    }
}
