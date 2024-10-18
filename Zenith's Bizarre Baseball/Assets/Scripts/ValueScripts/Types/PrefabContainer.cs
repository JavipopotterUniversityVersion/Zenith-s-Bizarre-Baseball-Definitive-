using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PrefabContainer", menuName = "Value/Prefab")]
public class PrefabContainer : ScriptableObject
{
    [SerializeField] GameObject _prefab;
    public GameObject Prefab => _prefab;
    UnityEvent _onPrefabChanged = new UnityEvent();
    public UnityEvent OnPrefabChanged => _onPrefabChanged;

    public void SetPrefab(GameObject obj)
    {
        _prefab = obj;
        OnPrefabChanged.Invoke();
    }
}
