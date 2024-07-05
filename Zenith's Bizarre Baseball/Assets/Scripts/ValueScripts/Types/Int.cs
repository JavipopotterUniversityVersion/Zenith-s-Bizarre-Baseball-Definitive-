using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Int", menuName = "Value/Int")]
public class Int : ScriptableObject
{
    [SerializeField] UnityEvent onValueChanged = new UnityEvent();
    public UnityEvent OnValueChanged => onValueChanged;

    [SerializeField] UnityEvent<int> onChangeLastValue = new UnityEvent<int>();
    public UnityEvent<int> OnChangeLastValue => onChangeLastValue;

    [SerializeField] int _value;
    public int Value
    {
        get => _value;
        set => _value = value;
    }

    int _lastValue;
    public int LastValue => _lastValue;

    public void SetValue(int value)
    {
        _lastValue = _value;
        onChangeLastValue.Invoke(_lastValue);
        
        _value = value;
        onValueChanged.Invoke();
    }

    public void AddValue(int value) => SetValue(Value + value);

    public void SubtractValue(int value) => SetValue(Value - value);

    public void MultiplyValue(int value) => SetValue(Value * value);

    public void DivideValue(int value) => SetValue(Value / value);
}