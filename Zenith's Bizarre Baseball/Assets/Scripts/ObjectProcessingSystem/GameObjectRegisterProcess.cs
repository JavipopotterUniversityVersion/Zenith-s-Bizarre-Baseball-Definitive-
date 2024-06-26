using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectRegisterProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] List<GameObject> _gameObjects = new List<GameObject>();
    public void Process(GameObject gameObject) => _gameObjects.Add(gameObject);
    public void RemoveAll() => _gameObjects.Clear();
    public void Remove(GameObject gameObject) => _gameObjects.Remove(gameObject);
    GameObject[] GetGameObjects() => _gameObjects.ToArray();

    [SerializeField] UnityEvent<GameObject[]> _onObjectCast = new UnityEvent<GameObject[]>();
    public void CastObjects() => _onObjectCast.Invoke(GetGameObjects());
}
