using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InstanceUnit
{
    [SerializeField] GameObject _instance;
    public GameObject Instance => _instance;

    [SerializeField] [Range(0, 1)] float _probability;
    public float Probability => _probability;

    public static GameObject GetInstance(InstanceUnit[] _instances)
    {
        float random = UnityEngine.Random.value;
        float sum = 0;
        foreach (var instance in _instances)
        {
            sum += instance.Probability;
            if (random < sum)
            {
                return instance.Instance;
            }
        }
        return null;
    }
}