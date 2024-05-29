using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "String", menuName = "Value/String")]
public class String : ScriptableObject
{
    [SerializeField] string _value;
    public string Value => _value;

    UnityEvent _onStringCall = new UnityEvent();
    public UnityEvent OnStringCall => _onStringCall;

    [SerializeField] SerializableDictionary<string, Int> _dictionary = new SerializableDictionary<string, Int>();

    public void SetString(string _string)
    {
        _value = _string;
       CallString();
    }

    public void CallString()
    {
        foreach (var item in _dictionary)
        {
            if (_value.Contains(item.Key))
            {
                _value = _value.Replace(item.Key, item.Value.Value.ToString());
            }
        }

        _onStringCall.Invoke();
    }
}
