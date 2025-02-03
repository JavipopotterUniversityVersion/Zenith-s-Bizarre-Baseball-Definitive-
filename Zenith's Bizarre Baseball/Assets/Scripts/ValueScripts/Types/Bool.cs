using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Bool", menuName = "Value/Bool")]
public class Bool : ScriptableICondition, ISaveable
{
    [SerializeField] bool _value;
    public bool Value => _value;

    [SerializeField] UnityEvent onValueChanged = new UnityEvent();
    public UnityEvent OnValueChanged => onValueChanged;
    [SerializeField] UnityEvent onTrue = new UnityEvent();
    [SerializeField] UnityEvent onFalse = new UnityEvent();
    [SerializeField] UnityEvent<bool> onChange = new UnityEvent<bool>();

    public void SetValue(bool value)
    {
        if(_value != value)
        {
            _value = value;
            onValueChanged.Invoke();

            if(value) onTrue.Invoke();
            else onFalse.Invoke();

            onChange.Invoke(value);
        }
    }

    public void SetInverseValue(bool value) => SetValue(!value);

    [ContextMenu("Refresh Value")]
    public void RefreshValue() => onValueChanged.Invoke();

    public void SetRawValue(bool value) => _value = value;

    public void ToggleValue() => SetValue(!_value);
    public override bool CheckCondition() => _value;

    string ISaveable.Key => name;
    public float SaveValue() => _value ? 1 : 0;
    public void LoadValue(float value) => _value = value == 1;
}