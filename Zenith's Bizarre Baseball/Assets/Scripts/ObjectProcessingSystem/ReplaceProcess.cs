using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReplaceProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] InstanceUnit[] _possibleReplacements;
    [SerializeField] UnityEvent<GameObject> onReplacedObject;

    public void Process(GameObject gameObject)
    {
        if(gameObject == null) return;

        GameObject replacementObject = InstanceUnit.GetRandomInstance(_possibleReplacements);
        Instantiate(replacementObject, gameObject.transform.position, gameObject.transform.rotation);
        onReplacedObject.Invoke(gameObject);

        Destroy(gameObject);
    }
}
