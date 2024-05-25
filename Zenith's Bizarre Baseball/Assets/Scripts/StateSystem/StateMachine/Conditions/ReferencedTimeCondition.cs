using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencedTimeCondition : MonoBehaviour, ICondition
{
     [SerializeField] Float timeToWait;
    float timePassed;

    public bool CheckCondition()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= timeToWait.Value)
        {
            timePassed = 0;
            return true;
        }
        return false;
    }

    private void OnValidate() => gameObject.name = $"Wait {timeToWait.name} ({timeToWait.Value}s)";
}
