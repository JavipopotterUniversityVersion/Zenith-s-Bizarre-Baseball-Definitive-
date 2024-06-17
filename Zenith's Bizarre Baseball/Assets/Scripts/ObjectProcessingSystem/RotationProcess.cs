using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] float _rotation;

    public void Process(GameObject gameObject)
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, _rotation);
    }
}