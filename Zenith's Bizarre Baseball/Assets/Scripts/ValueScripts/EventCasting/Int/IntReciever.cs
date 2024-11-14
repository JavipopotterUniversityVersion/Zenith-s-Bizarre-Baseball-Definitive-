using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntReciever : MonoBehaviour
{
    [SerializeField] Int _value;
    [SerializeField] int _threshold;

    [SerializeField] UnityEvent onLowValue = new UnityEvent();
    public UnityEvent OnLowValue => onLowValue;

    [SerializeField] UnityEvent onValue = new UnityEvent();
    public UnityEvent OnValue => onValue;

    [SerializeField] UnityEvent onHighValue = new UnityEvent();
    public UnityEvent OnHighValue => onHighValue;

    [SerializeField] UnityEvent<int> _onValueChanged;
    [SerializeField] UnityEvent<string> _onValueChangedAsString;

    private void Awake() {
        _value.OnValueChanged.AddListener(()=>_onValueChanged.Invoke(_value.Value));
        _value.OnValueChanged.AddListener(()=>_onValueChangedAsString.Invoke(_value.Value.ToString()));
        _onValueChangedAsString.Invoke(_value.Value.ToString());
    }

    private void OnDestroy() {
        _value.OnValueChanged.RemoveListener(()=>_onValueChanged.Invoke(_value.Value));
        _value.OnValueChanged.AddListener(()=>_onValueChangedAsString.Invoke(_value.Value.ToString()));
    }

    public void CheckValue() 
    {
        if(_value.Value < _threshold && _value.LastValue >= _threshold)
            onLowValue.Invoke();
        else if(_value.Value > _threshold && _value.LastValue <= _threshold)
            onHighValue.Invoke();
        else if(_value.Value == _threshold)
            onValue.Invoke();
    }
}
