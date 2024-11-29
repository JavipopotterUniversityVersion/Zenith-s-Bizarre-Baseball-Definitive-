using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MightAppearOrNot : MonoBehaviour
{
    [Range(0,1)] [SerializeField] float _appearProbability = 0.5f;

    void OnEnable()
    {
        if(Random.Range(0,1f) < 0.5) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
