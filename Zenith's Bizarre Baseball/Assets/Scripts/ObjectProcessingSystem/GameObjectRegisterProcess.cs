using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class GameObjectRegisterProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] List<GameObject> _gameObjects = new List<GameObject>();
    public void Process(GameObject gameObject)
    {
        gameObject.AddComponent<UnregisterOnDestroy>().SetRegisterProcess(this);
        _gameObjects.Add(gameObject);
    }
    public void RemoveAll() => _gameObjects.Clear();
    public void Remove(GameObject gameObject) => _gameObjects.Remove(gameObject);
    GameObject[] GetGameObjects() => _gameObjects.ToArray();

    public void DestroyObjects()
    {
        foreach (var gameObject in GetGameObjects()) Destroy(gameObject);
        RemoveAll();
    }

    [SerializeField] UnityEvent<GameObject[]> _onObjectCast = new UnityEvent<GameObject[]>();
    public void CastObjects() => _onObjectCast.Invoke(GetGameObjects());
}
