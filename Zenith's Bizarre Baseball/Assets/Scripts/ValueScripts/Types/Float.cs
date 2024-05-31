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
    [SerializeField] Processor _readProcessor;

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
public class Processor
{
    static string[] operationTypes = new string[] {"!=", "==", ">", "<", "||", "&&", "Random", "^", "*", "/", "%", "+", "-" };
    public string operation = "input";

    [Header("FLOAT REFERENCES")]
    [SerializeField] SerializableDictionary<string, Float> _floatDictionary = new SerializableDictionary<string, Float>();

    [Header("CURVE REFERENCES")]
    [SerializeField] SerializableDictionary<string, Curve> _curveDictionary = new SerializableDictionary<string, Curve>();

    [Header("OPERATION REFERENCES")]
    [SerializeField] SerializableDictionary<string, String> _operationDictionary = new SerializableDictionary<string, String>();

    [Header("FUNCTION REFERENCES")]
    [SerializeField] SerializableDictionary<string, Function> _functionDictionary = new SerializableDictionary<string, Function>();

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
        for(int i = startPoint; i < value.Length; i++)
        {
            if(value[i] == '(')
            {
                Replace(ref value, i + 1, input);
            }
        }
        string result = value.Substring(startPoint).Split(')')[0];
        value = value.Replace("(" + result + ")", Translate(result, input).ToString());
    }

    public float Result(float input)
    {
        if(operation == "input" || operation == "") return input;

        string value = operation;
        value = PlaceOperations(value);
        Replace(ref value, 0, input);
        return Translate(value, input);
    }

    string PlaceOperations(string value)
    {
        foreach(var item in _operationDictionary)
        {
            if(value.Contains(item.Key))
            {
                value = value.Replace(item.Key, item.Value.Value);
            }
        }
        return value;
    }

    float Translate(string value, float input)
    {
        value = value.Trim();

        if(ContainsOperations(value)) return Calculate(input, value);

        if(_floatDictionary.ContainsKey(value)) return _floatDictionary[value].Value;

        if(SearchCurve(value, out string curveName))
        {
            string curveInput = value.Split(" ")[1];
            return _curveDictionary[curveName].Evaluate(Translate(curveInput, input));
        }

        if(SearchFunction(value, out string functionName))
        {
            float functionInput = Translate(value.Split(" ")[1], input);
            return _functionDictionary[functionName].Result(functionInput);
        }

        if(value == "input") return input;

        if(float.TryParse(value, out float result)) return result;
        else return input;
    }

    bool SearchFunction(string value, out string key)
    {
        foreach(var function in _functionDictionary)
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

    bool SearchCurve(string value, out string key)
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

    bool ContainsOperations(string value)
    {
        bool contains = false;

        int i = 0;
        while(i < operationTypes.Length && !contains)
        {
            contains = value.Contains(operationTypes[i]);
            i++;
        }
        
        return contains;
    }

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
            case "%":
                return value1 % value2;
            case "^":
                return Mathf.Pow(value1, value2);
            case "Random":
                return UnityEngine.Random.Range(value1, value2);
            case "==":
                return value1 == value2 ? 1 : 0;
            case "!=":
                return value1 != value2 ? 1 : 0;
            case ">":
                return value1 > value2 ? 1 : 0;
            case "<":
                return value1 < value2 ? 1 : 0;
            case "&&":
                return value1 != 0 && value2 != 0 ? 1 : 0;
            case "||":
                return value1 != 0 || value2 != 0 ? 1 : 0;
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