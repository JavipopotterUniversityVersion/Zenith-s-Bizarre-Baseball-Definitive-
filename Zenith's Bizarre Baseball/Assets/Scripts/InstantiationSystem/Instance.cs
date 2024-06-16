using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class InstanceUnit
{
    [SerializeField] GameObject _instance;
    public GameObject Instance => _instance;

    [SerializeField] ObjectProcessor _appearCondition;
    public bool CanAppear => _appearCondition.ResultBool();

    [SerializeField] [Range(0, 1)] float _probability;
    public float Probability => _probability;

    public static GameObject GetInstance(InstanceUnit[] _instances)
    {
        List<InstanceUnit> instances = _instances.Where(instance => instance.CanAppear).ToList();

        float range = instances.Sum(instance => instance.Probability);
        float random = UnityEngine.Random.Range(0, range);
        float sum = 0;
        foreach (var instance in instances)
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