using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCondition : MonoBehaviour, ICondition, IReadable
{
    [SerializeField] float timeToWait;
    [SerializeField] ObjectProcessor _timeProcessor;
    float timePassed;

    float waitTime;

    public float Read() => timePassed;

    private void Awake() {
        waitTime = _timeProcessor.Result(timeToWait);
    }

    public bool CheckCondition()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= waitTime)
        {
            waitTime = _timeProcessor.Result(timeToWait);
            timePassed = 0;
            return true;
        }
        return false;
    }

    private void OnValidate() => gameObject.name = $"Wait {timeToWait}s";
    public void SetTime(float newTime) => timeToWait = newTime;
    public void RestartTimer() => timePassed = 0;
}
