using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Float", menuName = "Value/Float")]
public class Float : ScriptableObject
{
    [SerializeField] float _value;
    
    UnityEvent<float> onValueChanged = new UnityEvent<float>();
    public UnityEvent<float> OnValueChanged => onValueChanged;

    public float Value
    {
        get => _value;
        set => _value = value;
    }

    public void SetValue(float value) => Value = value;

    public void AddValue(float value) => Value += value;

    public void SubtractValue(float value) => Value -= value;

    public void MultiplyValue(float value) => Value *= value;

    public void DivideValue(float value) => Value /= value;
}