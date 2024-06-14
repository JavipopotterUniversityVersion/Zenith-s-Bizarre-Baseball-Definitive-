using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoolReceiver : MonoBehaviour
{
    [SerializeField] Bool _value;
    [SerializeField] bool checkOnAwake = false;

    [SerializeField] UnityEvent _onTrue = new UnityEvent();
    [SerializeField] UnityEvent _onFalse = new UnityEvent();

    private void Awake()
    {
        _value.OnValueChanged.AddListener(UpdateBool);
        if (checkOnAwake) UpdateBool();
    }

    private void OnDestroy()
    {
        _value.OnValueChanged.RemoveListener(UpdateBool);
    }

    void UpdateBool()
    {
        if (_value.Value)
        {
            _onTrue.Invoke();
        }
        else
        {
            _onFalse.Invoke();
        }
    }
}
