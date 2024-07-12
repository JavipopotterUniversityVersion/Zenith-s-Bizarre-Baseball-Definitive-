using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

[CreateAssetMenu(fileName = "Float", menuName = "Value/Float")]
public class Float : ScriptableICondition
{
    [SerializeField] float _value;
    [SerializeField] Processor _readProcessor;

    public float Value
    {
        get
        {
            onRead.Invoke();
            return _readProcessor.Result(_value);
        }
        set
        {
            _lastValue = _value;
            _value = value;
            onValueChanged.Invoke();
        }
    }

    float _lastValue;
    public float LastValue => _lastValue;

    [Space(10)]
    [Header("EVENT")]
    [Space(10)]

    [SerializeField] UnityEvent onRead = new UnityEvent();
    public UnityEvent OnRead => onRead;

    [SerializeField] UnityEvent onValueChanged = new UnityEvent();
    public UnityEvent OnValueChanged => onValueChanged;

    public void SetValue(float value) => Value = value;
    public void SetValue(Float value) => Value = value.Value;

    public void AddValue(float value) => SetValue(_value + value);

    public void SubtractValue(float value) => SetValue(_value - value);

    public void MultiplyValue(float value) => SetValue(_value * value);

    public void DivideValue(float value) => SetValue(_value / value);

    public override bool CheckCondition() => Value != 0;
    public float GetRawValue() => _value;
    public void SetRawValue(float value) => _value = value;

    [ContextMenu("Remove Listeners")]
    public void RemoveListeners()
    {
        onRead.RemoveAllListeners();
        onValueChanged.RemoveAllListeners();
    }
}



[Serializable]
public class Processor
{
    string _name = "";
    static string[] operationTypes = new string[] {"!=", "==", ">", "<", "<=", ">=", "&&", "||", "Random", "^", "*", "/", "%", "+", "-" };
    public string operation = "input";
    [SerializeField] bool _debug = false;
    [SerializeField] protected bool _showReferences;

    #region references

    [ConditionalField(nameof(_showReferences))]
    [SerializeField] SerializableDictionary<string, Float> _floatDictionary = new SerializableDictionary<string, Float>();

    [ConditionalField(nameof(_showReferences))]
    [SerializeField] SerializableDictionary<string, Int> _intDictionary = new SerializableDictionary<string, Int>();

    [ConditionalField(nameof(_showReferences))]
    [SerializeField] SerializableDictionary<string, Curve> _curveDictionary = new SerializableDictionary<string, Curve>();

    [ConditionalField(nameof(_showReferences))]
    [SerializeField] SerializableDictionary<string, String> _operationDictionary = new SerializableDictionary<string, String>();

    [ConditionalField(nameof(_showReferences))]
    [SerializeField] SerializableDictionary<string, Function> _functionDictionary = new SerializableDictionary<string, Function>();

    [ConditionalField(nameof(_showReferences))]
    [SerializeField] SerializableDictionary<string, String> _stringDictionary = new SerializableDictionary<string, String>();

    [ConditionalField(nameof(_showReferences))]
    [SerializeField] SerializableDictionary<string, Bool> _boolDictionary = new SerializableDictionary<string, Bool>();

    [ConditionalField(nameof(_showReferences))]
    [SerializeField] SerializableDictionary<string, UnityEvent> _eventDictionary = new SerializableDictionary<string, UnityEvent>();
    #endregion
    #region methods
    float Calculate(float input, string operation)
    {
        float result = 0;
        bool foundOperation = false;

        int i = operationTypes.Length - 1;
        while(i >= 0 && !foundOperation)
        {
            if(operation.Contains(operationTypes[i]) == false) { i--; continue; }

            string[] operations = operation.Split(operationTypes[i], 2);

            if(_debug)Debug.Log($"Trying to operate with {operationTypes[i]} in {operation}");

            if(operations[0] != "")
            {
                if(_debug)Debug.Log(operations[0] + $"[{operationTypes[i]}]" + operations[1]);
                foundOperation = true;
                result = Operate(Translate(operations[0], input), Translate(operations[1], input), operationTypes[i]);
            }
            else if(ContainsOperations(operations[1]) == false)
            {
                if(operationTypes[i] == "-") result = -Translate(operations[1], input);
            }

            i--;
        }

        return result;
    }

    void Replace(ref string value, int startPoint, float input)
    {
        if(value.Contains("(") == false) return;
        
        string readed = "";
        for(int i = startPoint; i < value.Length; i++)
        {
            if(value[i] == '(')
            {
                Replace(ref value, i + 1, input);
            }

            if(value[i] == ')')
            {
                value = value.Replace("(" + readed + ")", Translate(readed, input).ToString());
                break;
            }

            readed += value[i];
        }
    }

    public float Result(float input = 1) => ResultOf(operation, input);
    public int ResultInt(float input = 1) => (int) ResultOf(operation, input);

    public float ResultOf(string operation, float input)
    {
        _name = UnityEngine.Random.Range(0,999999).ToString();
        if(operation == "" || operation == "input") return input;

        string value = operation;
        value = PlaceOperations(value);
        Replace(ref value, 0, input);
        if(_debug) Debug.Log(value);
        return Translate(value, input);
    }

    public bool ResultBool(float input = 1) => Result(input) != 0;

    string PlaceOperations(string value)
    {
        foreach(var item in _operationDictionary)
        {
            foreach(var function in _functionDictionary)
            {
                string key = function.Key + " <= " + item.Key;

                if(value.Contains(key)) value = value.Replace(key, function.Value.Translate(item.Value.Value).ToString());
            }

            if(value.Contains(item.Key)) value = value.Replace(item.Key, item.Value.Value);
        }
        return value;
    }

    public float Translate(string value, float input)
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

        if(SearchEvent(value, out string eventName))
        {
            float eventInput = Translate(value.Split(" ")[1], input);
            bool success = eventInput != 0 ? true : false;
            if(success) _eventDictionary[eventName].Invoke();

            return eventInput;
        }

        if(_boolDictionary.ContainsKey(value)) return _boolDictionary[value].Value ? 1 : 0;
        if(_stringDictionary.ContainsKey(value)) return StringToFloat(_stringDictionary[value].Value);
        if(_intDictionary.ContainsKey(value)) return _intDictionary[value].Value;
        if(AdditionalOperations(out float newValue, value))
        {
            if(_debug) Debug.Log("The input " + value + " has been found, returning " + newValue);
            return newValue;
        }

        if(value == "input") return input;

        if(float.TryParse(value, out float result)) return result;
        else return StringToFloat(value);
    }

    public virtual bool AdditionalOperations(out float value, string inputValue)
    {
        value = 0;
        return false;
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

    bool SearchEvent(string value, out string key)
    {
        foreach(var function in _eventDictionary)
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
            case "<=":
                return value1 <= value2 ? 1 : 0;
            case ">=":
                return value1 >= value2 ? 1 : 0;
            default:
                return value1;
        }
    }

    float StringToFloat(string value)
    {
        string result = "";

        foreach(char c in value)
        {
            result += Convert.ToInt32(c).ToString();
        }

        if(_debug) Debug.Log(value + " is " + result + " we are in the processor:" + operation + "my name is " + _name);

        return float.Parse(result);
    }

    public void Subscribe(UnityAction action)
    {
        foreach(var item in _floatDictionary)
        {
            item.Value.OnValueChanged.AddListener(action);
        }
    }

    public void Unsubscribe(UnityAction action)
    {
        foreach(var item in _floatDictionary)
        {
            item.Value.OnValueChanged.RemoveListener(action);
        }
    }
    #endregion
}

[Serializable]
public class ObjectProcessor : Processor
{
    public override bool AdditionalOperations(out float returnValue, string value)
    {
        returnValue = 0;
        value = value.Trim();

        if(_conditionDictionary.ContainsKey(value))
        {
            returnValue = Condition.Value(_conditionDictionary[value]);
            return true;
        }

        if(ReadableRef.GetReference(_readablesDictionary, value, out IReadable readable))
        {
            returnValue = readable.Read();
            return true;
        }


        return false;
    }

    
    [ConditionalField(nameof(_showReferences))]
    [SerializeField] SerializableDictionary<string, Condition[]> _conditionDictionary = new SerializableDictionary<string, Condition[]>();

    [SerializeField] ReadableRef[] _readablesDictionary;

    [Serializable]
    class ReadableRef
    {
        public string Key;
        public IRef<IReadable> Value;

        public static bool GetReference(ReadableRef[] readables, string key, out IReadable readable)
        {
            int i = 0;
            readable = null;

            while(i < readables.Length && readable == null)
            {
                if(readables[i].Key == key) readable = readables[i].Value.I;
                i++;
            }

            return readable != null;
        }
    }
}

public interface IReadable
{
    public float Read();
}
