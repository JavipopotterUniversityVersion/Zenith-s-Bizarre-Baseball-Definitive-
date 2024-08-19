using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkinColor : MonoBehaviour
{
    private void Awake() {
        MaterialPropertyBlock block = new MaterialPropertyBlock();

        block.SetFloat("_num", Random.Range(0f,1f));
        block.SetFloat("_num3", Random.Range(0f,1f));

        GetComponent<Renderer>().SetPropertyBlock(block);
    }
}
