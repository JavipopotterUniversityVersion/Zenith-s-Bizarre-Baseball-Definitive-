using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatReceiver : MonoBehaviour
{
    [SerializeField] Float _value;
    [SerializeField] ObjectFloatEvent[] _floatEvents;
    bool _isSubscribed = false;

    private void Start() => Subscribe();

    private void OnDestroy() => Unsubscribe();

    public void CheckValue(float value) 
    {
        foreach (ObjectFloatEvent floatEvent in _floatEvents) floatEvent.Invoke(value);
    }

    public void Subscribe()
    {
        if(_isSubscribed) return;
        
        _value.OnValueChanged.AddListener(CheckValue);
        CheckValue(_value.Value);
    }

    public void Unsubscribe() => _value.OnValueChanged.RemoveListener(CheckValue);
}

[Serializable]
public struct ObjectFloatEvent
{
    [SerializeField] ObjectProcessor _processor;
    [SerializeField] UnityEvent _event;

    public readonly void Invoke(float input)
    {
        if(_processor.ResultBool(input)) _event.Invoke();
    }
}

[Serializable]
public struct FloatEvent
{
    [SerializeField] Processor _processor;
    [SerializeField] UnityEvent _event;

    public readonly void Invoke(float input)
    {
        if(_processor.ResultBool(input)) _event.Invoke();
    }
}