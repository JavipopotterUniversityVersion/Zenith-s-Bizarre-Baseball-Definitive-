using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RandomInstanceOnBlocks : MonoBehaviour, IBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] InstanceGroup[] instanceGroups;
    [SerializeField] InstanceUnit[] UniversalUnits;
    InstanceBlock[] globalInstanceBlocks;
    InstanceUnit[] selectedUnits;

    private void Awake() => globalInstanceBlocks = GetComponentsInChildren<InstanceBlock>(true);
    private void Start()
    {
        selectedUnits = new InstanceUnit[0];
        selectedUnits = selectedUnits.Concat(UniversalUnits).ToArray();
        selectedUnits = selectedUnits.Concat(InstanceGroup.GetRandomGroup(instanceGroups).instanceUnits).ToArray();
    }

    public void ExecuteBehaviour()
    {
        globalInstanceBlocks[UnityEngine.Random.Range(0,globalInstanceBlocks.Length)].Instance(selectedUnits, parent);
    }

    [Serializable]
    class InstanceGroup
    {
        public InstanceUnit[] instanceUnits;
        [Range(0,1)] public float probability;

        public static InstanceGroup GetRandomGroup(InstanceGroup[] groups)
        {
            float totalProbability = groups.Sum(group => group.probability);
            float randomValue = UnityEngine.Random.Range(0, totalProbability);
            float currentProbability = 0;

            foreach (var group in groups)
            {
                currentProbability += group.probability;
                if (randomValue <= currentProbability) return group;
            }

            return null;
        }
    }
}