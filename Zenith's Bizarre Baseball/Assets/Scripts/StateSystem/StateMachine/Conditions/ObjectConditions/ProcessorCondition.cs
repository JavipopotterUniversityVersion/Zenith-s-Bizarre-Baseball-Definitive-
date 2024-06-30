using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorCondition : MonoBehaviour, ICondition, IBehaviour
{
    [SerializeField] ObjectProcessor _processor;
    [SerializeField] bool onValidate = false;

    public bool CheckCondition() => _processor.ResultBool(1);
    public void ExecuteBehaviour() => CheckCondition();
    
    private void OnValidate() 
    {
        if(onValidate)
        {
            name = "(" + _processor.operation + ")" + " -> " + _processor.ResultBool(1).ToString();
        }
    }

    public void SetProcessor(ObjectProcessor processor) => _processor = processor;
}