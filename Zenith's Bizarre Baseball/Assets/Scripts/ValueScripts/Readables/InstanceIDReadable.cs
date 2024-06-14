using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceIDReadable : Readable
{
    [SerializeField] GameObject _instance;
    public GameObject Instance => _instance;

    public override float Read() => _instance.GetInstanceID();
    public void SetInstance(GameObject instance) => _instance = instance;
    public void SetInstance(Transform instance) => _instance = instance.gameObject;
}
