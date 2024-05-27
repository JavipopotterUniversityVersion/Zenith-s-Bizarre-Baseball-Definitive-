using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Float", menuName = "Value/Float")]
public class Float : ScriptableObject
{
    [SerializeField] float _value;
    public float Value
    {
        get => _value;
        set
        {
            _lastValue = _value;
            _value = value;
            onValueChanged.Invoke(_value);
        }
    }

    float _lastValue;
    public float LastValue => _lastValue;

    UnityEvent<float> onValueChanged = new UnityEvent<float>();
    public UnityEvent<float> OnValueChanged => onValueChanged;


    public void SetValue(float value) => Value = value;

    public void AddValue(float value) => Value += value;

    public void SubtractValue(float value) => Value -= value;

    public void MultiplyValue(float value) => Value *= value;

    public void DivideValue(float value) => Value /= value;
}