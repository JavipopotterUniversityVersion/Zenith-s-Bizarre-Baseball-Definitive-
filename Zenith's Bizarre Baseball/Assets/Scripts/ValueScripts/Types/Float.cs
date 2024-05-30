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
    [SerializeField] Operation _readProcessor;

    public float Value
    {
        get
        {
            return _readProcessor.Result(_value);
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

    public void SetValue(float value) => Value = value;

    public void AddValue(float value) => SetValue(_value + value);

    public void SubtractValue(float value) => SetValue(_value - value);

    public void MultiplyValue(float value) => SetValue(_value * value);

    public void DivideValue(float value) => SetValue(_value / value);
}

[Serializable]
public class Operation
{
    string[] operationTypes = new string[] { "*", "/", "+", "-" };
    public string operation = "input";
    [SerializeField] SerializableDictionary<string, Float> _floatDictionary = new SerializableDictionary<string, Float>();
    [SerializeField] SerializableDictionary<string, Curve> _curveDictionary = new SerializableDictionary<string, Curve>();

    float Calculate(float input, string operation)
    {
        float result = 0;
        bool foundOperation = false;

        int i = operationTypes.Length - 1;
        while(i >= 0 && !foundOperation)
        {
            string[] operations = operation.Split(operationTypes[i], 2);
            Debug.Log($"Trying to operate with {operationTypes[i]} in {operation}");

            if(operations.Length == 2)
            {
                Debug.Log(operations[0] + $"[{operationTypes[i]}]" + operations[1]);
                foundOperation = true;
                result = Operate(Translate(operations[0], input), Translate(operations[1], input), operationTypes[i]);
            }
            i--;
        }

        if(!foundOperation) result = Translate(operation, input);

        return result;
    }

    void Replace(ref string value, int startPoint, float input)
    {
        Debug.Log(value[startPoint]);
        for(int i = startPoint; i < value.Length; i++)
        {
            if(value[i] == '(')
            {
                Replace(ref value, i + 1, input);
            }
        }
        string result = value.Substring(startPoint).Split(')')[0];
        value = value.Replace("(" + result + ")", Translate(result, input).ToString());
        // Debug.Log(value);
    }

    public float Result(float input)
    {
        string value = operation;
        Replace(ref value, 0, input);
        Debug.Log(operation);
        return Translate(value, input);
    }

    float Translate(string value, float input)
    {
        value = value.Trim();

        if(ContainsOperations(value)) return Calculate(input, value);

        if(_floatDictionary.ContainsKey(value)) return _floatDictionary[value].Value;

        if(SearchFunction(value, out string curveName))
        {
            string curveInput = value.Split('<')[1].Split('>')[0].Trim();
            return _curveDictionary[curveName].Evaluate(Translate(curveInput, input));
        }

        if(value == "input") return input;

        if(float.TryParse(value, out float result)) return result;
        else return input;
    }

    bool SearchFunction(string value, out string key)
    {
        foreach(var function in _curveDictionary)
        {
            if(value.Contains(function.Key))
            {
                key = function.Key;
                return true;
            }
        }

        key = "";

        return false;
    }

    bool ContainsOperations(string value) => value.Contains("+") || value.Contains("-") || value.Contains("*") || value.Contains("/");

    float Operate(float value1, float value2, string operationType)
    {
        switch (operationType)
        {
            case "+":
                return value1 + value2;
            case "-":
                return value1 - value2;
            case "*":
                return value1 * value2;
            case "/":
                return value1 / value2;
            default:
                return value1;
        }
    }
}

// [Serializable]
// public class Modifier
// {
//     public enum ModifierType
//     {
//         Add,
//         Subtract,
//         Multiply,
//         Divide
//     }

//     [SerializeField] ModifierType _type;
//     [SerializeField] Float _value;

//     [Header("EXTRA MODIFIERS")]
//     [SerializeField] Modifier[] _extraModifiers;

//     [Header("CURVE MODIFIERS")]
//     [SerializeField] CurveModifier[] _curve;

//     public float Modify(float value)
//     {
//         switch (_type)
//         {
//             case ModifierType.Add:
//                 return value + CurveModifier.Modify(Modify(value, _extraModifiers), _curve);
//             case ModifierType.Subtract:
//                 return value - CurveModifier.Modify(Modify(value, _extraModifiers), _curve);
//             case ModifierType.Multiply:
//                 return value * CurveModifier.Modify(Modify(value, _extraModifiers), _curve);
//             case ModifierType.Divide:
//                 return value / CurveModifier.Modify(Modify(value, _extraModifiers), _curve);
//             default:
//                 return value;
//         }
//     }

//     public static float Modify(float value, Modifier[] modifiers)
//     {
//         foreach (var modifier in modifiers)
//         {
//             value = modifier.Modify(value);
//         }
//         return value;
//     }
// }

// [Serializable] public class CurveModifier
// {
//     [Header("CURVE")]
//     public Curve Curve;
//     [SerializeField] float _yMultiplier = 1;

//     [Header("RELATED VALUE")]
//     [SerializeField] Float _value;
//     [SerializeField] Modifier[] _modifiers;
//     [SerializeField] float _maxValue = 10;

//     public float Modify(float value)
//     {
//         return value * Curve.Evaluate(Modifier.Modify(_value.Value, _modifiers) / _maxValue) * _yMultiplier;
//     }

//     public static float Modify(float value, CurveModifier[] modifiers)
//     {
//         foreach (var modifier in modifiers)
//         {
//             value = modifier.Modify(value);
//         }
//         return value;
//     }
// }