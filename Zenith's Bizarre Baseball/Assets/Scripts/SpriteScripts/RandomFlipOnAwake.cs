using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFlipOnAwake : MonoBehaviour
{
    [SerializeField] bool flipX = true;
    [SerializeField] bool flipY = false;

    private void Awake() 
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if(flipX) sr.flipX = Random.value > 0.5f;
        if(flipY) sr.flipY = Random.value > 0.5f;
    }
}
