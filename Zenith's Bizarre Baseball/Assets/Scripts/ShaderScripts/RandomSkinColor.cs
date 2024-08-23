using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkinColor : MonoBehaviour
{
    private void Awake() {
        MaterialPropertyBlock block = new MaterialPropertyBlock();

        block.SetFloat("_SkinNum", Random.Range(0f,1f));
        block.SetFloat("_HairNum", Random.Range(0f,1f));

        GetComponent<Renderer>().SetPropertyBlock(block);
    }
}
