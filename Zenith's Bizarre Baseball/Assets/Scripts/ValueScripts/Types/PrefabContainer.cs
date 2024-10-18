using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PrefabContainer", menuName = "Value/Prefab")]
public class PrefabContainer : ScriptableObject
{
    [SerializeField] GameObject _prefab;
    public GameObject Prefab => _prefab;
    UnityEvent<GameObject> _onPrefabChanged = new UnityEvent<GameObject>();
    public UnityEvent<GameObject> OnPrefabChanged => _onPrefabChanged;

    public void SetPrefab(GameObject obj)
    {
        _prefab = obj;
        OnPrefabChanged.Invoke(_prefab);
    }
}
