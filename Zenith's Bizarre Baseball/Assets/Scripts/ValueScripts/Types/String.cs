using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "String", menuName = "Value/String")]
public class String : ScriptableObject
{
    [SerializeField] string _value;
    public string Value => _stringProcessor.Process(_value);

    UnityEvent _onStringCall = new UnityEvent();
    public UnityEvent OnStringCall => _onStringCall;

    [SerializeField] StringProcessor _stringProcessor;

    public void SetString(string _string)
    {
        _value = _string;
       CallString();
    }

    public void SetString(String _string)
    {
        _value = _string.Value;
        CallString();
    }

    public void CallString()
    {
        _onStringCall.Invoke();
    }
}

[Serializable]
public struct ProcessedFloat
{
    [SerializeField] Float value;

    public Processor processor;
    public readonly float Value => processor.Result(value.Value);
}

[Serializable]
public class StringProcessor
{
    [SerializeField] SerializableDictionary<string, Int> _intDictionary = new SerializableDictionary<string, Int>();
    [SerializeField] SerializableDictionary<string, ProcessedFloat> _floatDictionary = new SerializableDictionary<string, ProcessedFloat>();
    [SerializeField] SerializableDictionary<string, String> _stringDictionary = new SerializableDictionary<string, String>();

    public string Process(string value)
    {
        foreach (var item in _stringDictionary)
        {
            if (value.Contains(item.Key))
            {
                value = value.Replace(item.Key, item.Value.Value);
            }
        }

        foreach (var item in _intDictionary)
        {
            if (value.Contains(item.Key))
            {
                value = value.Replace(item.Key, item.Value.Value.ToString());
            }
        }

        foreach (var item in _floatDictionary)
        {
            if (value.Contains(item.Key))
            {
                value = value.Replace(item.Key, item.Value.Value.ToString());
            }
        }

        return value;
    }
}
