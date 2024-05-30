using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Float", menuName = "Value/Float")]
public class Float : ScriptableObject
{
    [SerializeField] float _value;
    [SerializeField] Modifier[] _readModifiers;

    public float Value
    {
        get
        {
            return Modifier.Modify(_value, _readModifiers);
        }
        set
        {
            _lastValue = _value;
            _value = value;
            onValueChanged.Invoke(_value);
        }
    }

    float _lastValue;
    public float LastValue => _lastValue;

    [Space(10)]
    [Header("EVENT")]
    [Space(10)]

    [SerializeField] UnityEvent<float> onValueChanged = new UnityEvent<float>();
    public UnityEvent<float> OnValueChanged => onValueChanged;

    [Space(10)]
    [Header("OPERATION MODIFIERS")]
    [Space(10)]

    [SerializeField] Modifier[] _setModifiers;
    public void SetValue(float value) => Value = Modifier.Modify(value, _setModifiers);

    [Space(2)]
    [SerializeField] Modifier[] _addModifiers;
    public void AddValue(float value) => Value += Modifier.Modify(value, _addModifiers);

    [Space(2)]
    [SerializeField] Modifier[] _subtractModifiers;
    public void SubtractValue(float value) => Value -= Modifier.Modify(value, _subtractModifiers);

    [Space(2)]
    [SerializeField] Modifier[] _multiplyModifiers;
    public void MultiplyValue(float value) => Value *= Modifier.Modify(value, _multiplyModifiers);

    [Space(2)]
    [SerializeField] Modifier[] _divideModifiers;
    public void DivideValue(float value) => Value /= Modifier.Modify(value, _divideModifiers);
}

// [Serializable]
// public class Modifier

[Serializable]
public class Modifier
{
    public enum ModifierType
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }

    [SerializeField] ModifierType _type;
    [SerializeField] Float _value;

    [Header("EXTRA MODIFIERS")]
    [SerializeField] Modifier[] _extraModifiers;

    [Header("CURVE MODIFIERS")]
    [SerializeField] CurveModifier[] _curve;

    public float Modify(float value)
    {
        switch (_type)
        {
            case ModifierType.Add:
                return value + CurveModifier.Modify(Modify(value, _extraModifiers), _curve);
            case ModifierType.Subtract:
                return value - CurveModifier.Modify(Modify(value, _extraModifiers), _curve);
            case ModifierType.Multiply:
                return value * CurveModifier.Modify(Modify(value, _extraModifiers), _curve);
            case ModifierType.Divide:
                return value / CurveModifier.Modify(Modify(value, _extraModifiers), _curve);
            default:
                return value;
        }
    }

    public static float Modify(float value, Modifier[] modifiers)
    {
        foreach (var modifier in modifiers)
        {
            value = modifier.Modify(value);
        }
        return value;
    }
}

[Serializable] public class CurveModifier
{
    [Header("CURVE")]
    public Curve Curve;
    [SerializeField] float _yMultiplier = 1;

    [Header("RELATED VALUE")]
    [SerializeField] Float _value;
    [SerializeField] Modifier[] _modifiers;
    [SerializeField] float _maxValue = 10;

    public float Modify(float value)
    {
        return value * Curve.Evaluate(Modifier.Modify(_value.Value, _modifiers) / _maxValue) * _yMultiplier;
    }

    public static float Modify(float value, CurveModifier[] modifiers)
    {
        foreach (var modifier in modifiers)
        {
            value = modifier.Modify(value);
        }
        return value;
    }
}