using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] GameObject[] objectsToAdd;
    public void Process(GameObject gameObject)
    {
        foreach(GameObject obj in objectsToAdd)
        {
            Instantiate(obj, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        }
    }
}
