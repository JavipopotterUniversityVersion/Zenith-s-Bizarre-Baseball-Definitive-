using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] InstanceUnit[] _possibleReplacements;

    public void Process(GameObject gameObject)
    {
        GameObject replacementObject = InstanceUnit.GetRandomInstance(_possibleReplacements);
        Instantiate(replacementObject, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
