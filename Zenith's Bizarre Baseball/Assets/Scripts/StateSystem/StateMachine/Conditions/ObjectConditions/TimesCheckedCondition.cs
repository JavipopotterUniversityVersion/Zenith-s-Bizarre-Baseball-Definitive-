using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimesCheckedCondition : MonoBehaviour, ICondition
{
    int timesChecked = 0;
    [SerializeField] int timesToCheck = 5;

    public bool CheckCondition()
    {
        timesChecked++;

        if(timesChecked >= timesToCheck)
        {
            timesChecked = 0;
            return true;
        }
        return false;
    }

    private void OnValidate() {
        name = $"Check {timesToCheck} times";
    }
}
