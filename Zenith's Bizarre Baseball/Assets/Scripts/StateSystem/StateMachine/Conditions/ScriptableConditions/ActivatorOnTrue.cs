using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ConditionStack", menuName = "Conditions/Activator")]
public class ActivatorOnTrue : ScriptableObject
{
    [SerializeField] Processor _processor;
    [SerializeField] UnityEvent _onTrue = new UnityEvent();

    public void Check()
    {
        if(_processor.ResultBool(1)) _onTrue.Invoke();
    }
}
