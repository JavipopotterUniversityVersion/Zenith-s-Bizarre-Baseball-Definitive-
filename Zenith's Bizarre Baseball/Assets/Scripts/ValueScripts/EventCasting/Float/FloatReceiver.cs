using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatReceiver : MonoBehaviour
{
    [SerializeField] Float _value;
    [SerializeField] ObjectFloatEvent[] _floatEvents;

    private void Awake() => Subscribe();
    private void Start() => CheckValue();

    private void OnDestroy() => Unsubscribe();

    public void CheckValue() 
    {
        foreach (ObjectFloatEvent floatEvent in _floatEvents) floatEvent.Invoke(_value.Value);
    }

    public void Subscribe()
    {
        _value.OnValueChanged.AddListener(CheckValue);
        CheckValue();
    }

    public void Unsubscribe() => _value.OnValueChanged.RemoveListener(CheckValue);
}

[Serializable]
public struct ObjectFloatEvent
{
    [SerializeField] ObjectProcessor _processor;
    [SerializeField] UnityEvent _event;

    public readonly void Subscribe(UnityAction action) => _processor.Subscribe(action);
    public readonly void Unsubscribe(UnityAction action) => _processor.Unsubscribe(action);

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

    public readonly void Subscribe(UnityAction action) => _processor.Subscribe(action);
    public readonly void Unsubscribe(UnityAction action) => _processor.Unsubscribe(action);

    public readonly void Invoke(float input)
    {
        if(_processor.ResultBool(input)) _event.Invoke();
    }
}