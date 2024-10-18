using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetReceiver : MonoBehaviour
{
    [SerializeField] PrefabContainer prefabContainer;
    GameObject _currentObject;

    private void Awake() 
    {
        prefabContainer.OnPrefabChanged.AddListener(OnObjectChange);
        OnObjectChange();
    }

    void OnObjectChange()
    {
        if(_currentObject != null)
        {
            Destroy(_currentObject);
            if(prefabContainer.Prefab != null)
            {
                _currentObject = Instantiate(prefabContainer.Prefab, transform.position, transform.rotation, transform);
            }
        }
    }

    private void OnDestroy() {
        prefabContainer.OnPrefabChanged.RemoveListener(OnObjectChange);
    }
}
