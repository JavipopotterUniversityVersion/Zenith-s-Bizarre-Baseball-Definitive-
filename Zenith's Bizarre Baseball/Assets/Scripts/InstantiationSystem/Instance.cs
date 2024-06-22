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

    [SerializeField] IRef<IGameObjectProcessor>[] _processors;
    public IGameObjectProcessor[] Processors => IGameObjectProcessor.ToArray(_processors);

    [SerializeField] [Range(0, 1)] float _probability;
    public float Probability => _probability;

    public static InstanceUnit GetRandomUnit(InstanceUnit[] _instances)
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
                return instance;
            }
        }
        return null;
    }
    public static GameObject GetRandomInstance(InstanceUnit[] _instances) => GetRandomUnit(_instances).Instance;

    public static InstanceUnit GetFirstAvailableInstance(InstanceUnit[] _instances, int _startIndex = 0)
    {
        InstanceUnit instance = null;
        int i = _startIndex;

        int lastIndex = _startIndex - 1;
        if(lastIndex < 0) lastIndex = _instances.Length;

        if(_instances[lastIndex] == null)
        {
            Debug.LogError("Last index Instance is null, please check the array.");
            return null;
        }

        while (instance == null)
        {
            if (_instances[i].CanAppear || i == lastIndex)
            {
                instance = _instances[i];
            }
            i++;

            if (i >= _instances.Length) i = 0;
        }

        return instance;
    }
    public static GameObject GetFirstAvailableInstanceGameObject(InstanceUnit[] _instances) => GetFirstAvailableInstance(_instances).Instance;
}