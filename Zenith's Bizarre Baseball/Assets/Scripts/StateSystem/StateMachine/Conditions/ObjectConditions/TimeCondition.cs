using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCondition : MonoBehaviour, ICondition
{
    [SerializeField] float timeToWait;
    float timePassed;

    public bool CheckCondition()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= timeToWait)
        {
            timePassed = 0;
            return true;
        }
        return false;
    }

    private void OnValidate() => gameObject.name = $"Wait {timeToWait}s";
    public void SetTime(float newTime) => timeToWait = newTime;
}
