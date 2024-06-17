using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GenericReference : MonoBehaviour
{
    [SerializeField] SerializableDictionary<string, IRef<Object>> _reference;
    public Object GetReference(string key) => _reference[key].I;
    
    public void AddReferences(SerializableDictionary<string, IRef<Object>> references)
    {
        foreach (KeyValuePair<string, IRef<Object>> reference in references)
        {
            _reference.Add(reference.Key, reference.Value);
        }
    }
    public void AddReference(string key, IRef<Object> reference) => _reference.Add(key, reference);

    public void RemoveReference(string key) => _reference.Remove(key);
}
