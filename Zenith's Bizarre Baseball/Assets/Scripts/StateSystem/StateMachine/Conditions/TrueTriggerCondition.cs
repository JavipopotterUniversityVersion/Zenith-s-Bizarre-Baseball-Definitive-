using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueTriggerCondition : MonoBehaviour, ICondition
{
    bool triggerValue;

    public bool CheckCondition()
    {
        if (triggerValue)
        {
            triggerValue = false;
            return true;
        }
        return false;
    }

    public void SetTrigger() => triggerValue = true;

    private void OnValidate() 
    {
        if(triggerValue) name = "|+|";
        else name = "| |";
    }
}
