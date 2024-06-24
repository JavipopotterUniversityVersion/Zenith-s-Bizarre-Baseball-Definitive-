using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] float _rotation;
    [SerializeField] ObjectProcessor _processor;

    public void Process(GameObject gameObject)
    {
        float rotation = _processor.Result(_rotation);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}