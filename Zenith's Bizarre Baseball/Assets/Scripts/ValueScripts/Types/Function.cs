using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Function", menuName = "Value/Function")]
public class Function : ScriptableObject
{
    [SerializeField] Processor _processor;

    public Processor Processor => _processor;

    public float Result(float input) => _processor.Result(input);
}
