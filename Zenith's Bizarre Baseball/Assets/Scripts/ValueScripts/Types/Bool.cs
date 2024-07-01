using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Bool", menuName = "Value/Bool")]
public class Bool : ScriptableICondition
{
    [SerializeField] bool _value;
    public bool Value => _value;

    [SerializeField] UnityEvent onValueChanged = new UnityEvent();
    public UnityEvent OnValueChanged => onValueChanged;

    public void SetValue(bool value)
    {
        if(_value != value)
        {
            _value = value;
            onValueChanged.Invoke();
        }
    }

    public void ToggleValue() => SetValue(!_value);
    public override bool CheckCondition() => _value;

    [SerializeField] bool _originalValue;
    public void ResetValue() => SetValue(_originalValue);
}