using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTimeCondition : MonoBehaviour, ICondition
{
    [SerializeField] float minTime;
    [SerializeField] float maxTime;
    float timeToWait;

    private void Start() => timeToWait = Random.Range(minTime, maxTime);

    public bool CheckCondition()
    {
        timeToWait -= Time.deltaTime;
        if(timeToWait <= 0)
        {
            timeToWait = Random.Range(minTime, maxTime);
            return true;
        }
        return false;
    }

    private void OnValidate() => gameObject.name = $"Wait {minTime}s to {maxTime}s";
}