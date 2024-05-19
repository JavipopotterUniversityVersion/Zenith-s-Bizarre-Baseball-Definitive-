using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueCondition : MonoBehaviour, ICondition
{
    [SerializeField] bool value = true;
    public bool CheckCondition() => value;

    private void OnValidate() {
        name = value ? "True" : "False";
    }

    public void SetValue(bool newValue) => value = newValue;
}