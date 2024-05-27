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
