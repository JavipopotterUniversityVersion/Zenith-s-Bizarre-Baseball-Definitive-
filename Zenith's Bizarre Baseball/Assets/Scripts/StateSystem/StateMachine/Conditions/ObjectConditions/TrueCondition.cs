using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrueCondition : MonoBehaviour, ICondition
{
    UnityEvent onValueChange = new UnityEvent();
    public UnityEvent OnValueChange => onValueChange;

    [SerializeField] bool value = true;

    public bool CheckCondition() => value;

    private void OnValidate() {
        name = value ? "True" : "False";
    }

    public void SetValue(bool newValue)
    {
        value = newValue;
        onValueChange.Invoke();
    }
}