using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceIDReadable : MonoBehaviour, IReadable
{
    [SerializeField] GameObject _instance;
    public GameObject Instance => _instance;

    public float Read() => _instance.GetInstanceID();
    public void SetInstance(GameObject instance) => _instance = instance;
    public void SetInstance(Transform instance) => _instance = instance.gameObject;
}
