using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericReference : MonoBehaviour
{
    [SerializeField] IRef<Object>[] _reference;
    public void SetReference(IRef<Object>[] reference) => _reference = reference;
    public Object[] GetReference() => IRef<Object>.ToArray(_reference);
    public Object GetFirstReference() => IRef<Object>.ToArray(_reference)[0];
    public Object GetReferenceOfType<T>()
    {
        foreach (Object reference in GetReference())
        {
            if (reference is T)
            {
                return reference;
            }
        }
        return null;
    }
    public void SetReference(Object[] reference) => _reference = IRef<Object>.ToRef(reference);
}
