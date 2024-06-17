using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassReferenceProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] IRef<Object>[] _references;

    public void Process(GameObject gameObject)
    {
        if(gameObject.TryGetComponent(out GenericReference reference)) reference.SetReference(_references);
    }
}
