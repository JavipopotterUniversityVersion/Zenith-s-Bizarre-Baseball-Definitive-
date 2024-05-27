using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatReceiver : MonoBehaviour
{
    [SerializeField] Float _value;
    [SerializeField] float _threshold;

    [SerializeField] UnityEvent onLowValue = new UnityEvent();
    public UnityEvent OnLowValue => onLowValue;

    [SerializeField] UnityEvent onValue = new UnityEvent();
    public UnityEvent OnValue => onValue;

    [SerializeField] UnityEvent onHighValue = new UnityEvent();
    public UnityEvent OnHighValue => onHighValue;

    bool _isSubscribed;

    private void Start() => Subscribe();

    private void OnDestroy() => Unsubscribe();

    public void CheckValue(float value) 
    {
        if(value < _threshold && _value.LastValue >= _threshold)
            onLowValue.Invoke();
        else if(value > _threshold && _value.LastValue <= _threshold)
            onHighValue.Invoke();
        else if(value == _threshold)
            onValue.Invoke();
    }

    public void Subscribe()
    {
        if(_isSubscribed) return;
        
        _value.OnValueChanged.AddListener(CheckValue);
        CheckValue(_value.Value);
    }

    public void Unsubscribe() => _value.OnValueChanged.RemoveListener(CheckValue);
}
