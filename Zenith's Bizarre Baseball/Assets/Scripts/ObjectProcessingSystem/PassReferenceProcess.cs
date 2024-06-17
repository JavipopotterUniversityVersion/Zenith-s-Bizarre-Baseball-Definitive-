using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassReferenceProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] SerializableDictionary<string, IRef<Object>> _references;

    public void Process(GameObject gameObject)
    {
        if(gameObject.TryGetComponent(out GenericReference reference)) reference.AddReferences(_references);
    }
}
