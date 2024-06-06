using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorCondition : MonoBehaviour, ICondition
{
    [SerializeField] ObjectProcessor _processor;

    public bool CheckCondition() => _processor.ResultBool(1);
    private void OnValidate() {
        name = "(" + _processor.operation + ")" + " -> " + _processor.ResultBool(1).ToString();
    }
}