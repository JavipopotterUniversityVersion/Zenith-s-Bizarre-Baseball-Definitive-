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

    public void SetString(string _string)
    {
        _value = _string;
        _onStringCall.Invoke();
    }

    public void CallString() => _onStringCall.Invoke();
}
