using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataCreator : MonoBehaviour
{
    [SerializeField] SaveSet[] _saveSets;

    private void Awake()
    {
        foreach (SaveSet saveSet in _saveSets)
        {
        }
    }
}
