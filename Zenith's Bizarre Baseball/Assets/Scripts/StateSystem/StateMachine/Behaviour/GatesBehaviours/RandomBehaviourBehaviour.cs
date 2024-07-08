using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RandomBehaviourBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] PerformerUnit[] behaviours;
    [SerializeField] bool _onValidate = true;

    private void Awake() {
        foreach (var behaviour in behaviours)
        {
            behaviour.Initialize();
        }
    }

    public void ExecuteBehaviour()
    {
        PerformerUnit[] availablePerformers = behaviours.Where(performer => performer.CheckConditions()).ToArray();
        if(availablePerformers.Length == 0) return;

        float range = availablePerformers.Sum(performer => performer.Probability);

        float random = UnityEngine.Random.Range(0, range);
        float sum = 0;
        foreach (var behaviourUnit in behaviours)
        {
            sum += behaviourUnit.Probability;
            if (random < sum)
            {
                behaviourUnit.ExecuteBehaviours();
                if(behaviourUnit.autoRemove) behaviours = behaviours.Where(performer => performer != behaviourUnit).ToArray();
                return;
            }
        }
    }

    private void OnValidate() 
    {
        if(_onValidate) name = "RandomBehaviour";
    }
}

[Serializable]
public class PerformerUnit
{
    [SerializeField] BehaviourPerformer behaviourPerformer;
    public bool CheckConditions() => behaviourPerformer.CheckConditions();
    public void ExecuteBehaviours() => behaviourPerformer.ExecuteBehaviours();

    public void Initialize() => behaviourPerformer.Initialize();

    public bool autoRemove = false;

    [SerializeField] [Range(0, 1)] float _probability;
    public float Probability => _probability;
}
