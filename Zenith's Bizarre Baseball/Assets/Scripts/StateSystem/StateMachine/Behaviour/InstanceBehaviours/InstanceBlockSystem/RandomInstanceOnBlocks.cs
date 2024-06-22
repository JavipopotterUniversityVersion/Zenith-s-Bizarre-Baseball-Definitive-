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


    // [Serializable]
    // class InstanceBlockData
    // {
    //     public InstanceBlock instanceBlock;
    //     [SerializeField] [Range(0, 1)] float probability = 1;

    //     public InstanceBlockData(InstanceBlock instanceBlock, float probability = 0)
    //     {
    //         this.instanceBlock = instanceBlock;
    //         this.probability = probability;
    //     }

    //     public static InstanceBlockData[] GetBlockDatas(InstanceBlock[] blocks)
    //     {
    //         InstanceBlockData[] blockDatas = new InstanceBlockData[blocks.Length];
    //         for (int i = 0; i < blocks.Length; i++)
    //         {
    //             blockDatas[i] = new InstanceBlockData(blocks[i]);
    //         }
    //         return blockDatas;
    //     }

    //     public static InstanceBlock GetRandomBlock(InstanceBlockData[] blocks)
    //     {
    //         float range = 0;
    //         foreach (var block in blocks) range += block.probability;

    //         float random = UnityEngine.Random.Range(0, range);
    //         float sum = 0;
    //         foreach (var block in blocks)
    //         {
    //             sum += block.probability;
    //             if (random < sum)
    //             {
    //                 return block.instanceBlock;
    //             }
    //         }
    //         return null;
    //     }
    // }