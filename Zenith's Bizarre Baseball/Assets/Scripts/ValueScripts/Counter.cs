using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Counter", menuName = "Value/Counter")]
public class Counter : ScriptableObject
{
    [SerializeField] Int _money;
    [SerializeField] int _threshold;

    [SerializeField] UnityEvent onLowValue = new UnityEvent();
    public UnityEvent OnLowValue => onLowValue;

    [SerializeField] UnityEvent onHighValue = new UnityEvent();
    public UnityEvent OnHighValue => onHighValue;

    public void CheckValue() 
    {
        if(_money.Value <= _threshold && _money.LastValue > _threshold)
            onLowValue.Invoke();
        else if(_money.LastValue < _threshold)
            onHighValue.Invoke();
    }
}
