using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityCondition : MonoBehaviour, ICondition
{
    [Range(0, 100)]
    [SerializeField] float probability;
    public bool CheckCondition()
    {
        return Random.Range(0, 100) < probability;
    }
    private void OnValidate() => gameObject.name = $"Probability {probability}%";
    public void SetProbability(float newProbability) => probability = newProbability;
}