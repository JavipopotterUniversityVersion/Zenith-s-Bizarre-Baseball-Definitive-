using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInstanceOnBlocks : MonoBehaviour, IBehaviour
{
    [SerializeField] InstanceUnit[] globalInstanceUnits;
    InstanceBlock[] globalInstanceBlocks;

    private void Awake() => globalInstanceBlocks = GetComponentsInChildren<InstanceBlock>(true);
    public void ExecuteBehaviour()
    {
        globalInstanceBlocks[UnityEngine.Random.Range(0,globalInstanceBlocks.Length)].Instance(globalInstanceUnits);
    }
}