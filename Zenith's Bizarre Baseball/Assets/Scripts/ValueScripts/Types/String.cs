using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void AddString(string value) => _value += value;
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
    [SerializeField] SerializableDictionary<string, StringFunction> _stringFunctionDictionary = new SerializableDictionary<string, StringFunction>();
    SerializableDictionary<string, Processor> _processorDictionary = new SerializableDictionary<string, Processor>();

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

        foreach (var item in _processorDictionary)
        {
            if (value.Contains(item.Key))
            {
                value = value.Replace(item.Key, item.Value.Result(0).ToString());
            }
        }

        foreach (var item in _stringFunctionDictionary)
        {
            if (value.Contains(item.Key))
            {
                string input = value.Split(item.Key + "(")[1].Split(')')[0];
                value = value.Replace(item.Key + "(" + input + ")", item.Value.Result(input));
            }
        }

        return value;
    }
}