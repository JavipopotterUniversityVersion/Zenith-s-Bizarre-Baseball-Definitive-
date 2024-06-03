using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBehaviourBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] BehaviourUnit[] behaviours;

    private void Awake() => BehaviourUnit.Initialize(behaviours);

    public void ExecuteBehaviour()
    {
        float random = UnityEngine.Random.value;
        float sum = 0;
        foreach (var behaviourUnit in behaviours)
        {
            sum += behaviourUnit.Probability;
            if (random < sum)
            {
                behaviourUnit.Behaviour.ExecuteBehaviour();
                return;
            }
        }
    }

    private void OnValidate() 
    {
        name = "RandomBehaviour";
    }
}

[Serializable]
public class BehaviourUnit
{
    [SerializeField] GameObject _behaviourContainer;

    IBehaviour _behaviour;
    public IBehaviour Behaviour => _behaviour;

    public static void Initialize(BehaviourUnit[] units)
    {
        foreach (var unit in units)
        {
            unit._behaviour = unit._behaviourContainer.GetComponent<IBehaviour>();
        }
    }

    [SerializeField] [Range(0, 1)] float _probability;
    public float Probability => _probability;
}
