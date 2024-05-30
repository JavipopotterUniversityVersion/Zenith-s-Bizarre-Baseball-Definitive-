using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "String", menuName = "Value/String")]
public class String : ScriptableObject
{
    [SerializeField] string _value;
    public string Value => SetReferences();

    UnityEvent _onStringCall = new UnityEvent();
    public UnityEvent OnStringCall => _onStringCall;

    [SerializeField] SerializableDictionary<string, Int> _intDictionary = new SerializableDictionary<string, Int>();
    [SerializeField] SerializableDictionary<string, Float> _floatDictionary = new SerializableDictionary<string, Float>();
    [SerializeField] SerializableDictionary<string, String> _stringDictionary = new SerializableDictionary<string, String>();

    public void SetString(string _string)
    {
        _value = _string;
       CallString();
    }

    public void CallString()
    {
        _onStringCall.Invoke();
    }

    string SetReferences()
    {
        string value = _value;

        foreach (var item in _intDictionary)
        {
            if (_value.Contains(item.Key))
            {
                value = _value.Replace(item.Key, item.Value.Value.ToString());
            }
        }

        foreach (var item in _floatDictionary)
        {
            if (_value.Contains(item.Key))
            {
                value = _value.Replace(item.Key, item.Value.Value.ToString());
            }
        }

        foreach (var item in _stringDictionary)
        {
            if (_value.Contains(item.Key))
            {
                value = _value.Replace(item.Key, item.Value.Value);
            }
        }

        return value;
    }

    // [System.Serializable]
    // struct Mod
    // {
    //     [SerializeField] Float _value;
    //     public readonly float Value => Modifier.Modify(_value.Value, _modifiers);
    //     [SerializeField] Modifier[] _modifiers;
    // }
}
