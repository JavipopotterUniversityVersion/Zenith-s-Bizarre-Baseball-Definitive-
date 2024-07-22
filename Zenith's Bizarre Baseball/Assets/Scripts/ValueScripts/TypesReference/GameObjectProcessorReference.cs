using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectProcessorReference : MonoBehaviour
{
    [SerializeField] IRef<IGameObjectProcessor>[] _processors;

    public void Process(GameObject gameObject)
    {
        foreach (IRef<IGameObjectProcessor> processor in _processors)
        {
            processor.I.Process(gameObject);
        }
    }

    public void Process(GameObject[] gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            Process(gameObject);
        }
    }

    public void ProcessRandom(GameObject[] gameObjects)
    {
        if(gameObjects.Length == 0) return;

        int randomValue = Random.Range(0, gameObjects.Length);
        GameObject randomObject = gameObjects[randomValue];

        Process(randomObject);
    }
}
