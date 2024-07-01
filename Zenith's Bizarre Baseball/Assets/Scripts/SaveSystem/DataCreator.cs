using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataCreator : MonoBehaviour
{
    [SerializeField] SaveSet _saveSet;

    private void Awake()
    {
        if(!File.Exists(_saveSet.Path))
        {
            File.Create(_saveSet.Path).Close();
            _saveSet.Write();
        }
    }
}
