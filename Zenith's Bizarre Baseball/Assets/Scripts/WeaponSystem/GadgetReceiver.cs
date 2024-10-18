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
    }

    void OnObjectChange(GameObject obj)
    {
        if(_currentObject != null)
        {
            Destroy(_currentObject);
            _currentObject = Instantiate(obj, transform.position, transform.rotation, transform);
        }
    }

    private void OnDestroy() {
        prefabContainer.OnPrefabChanged.RemoveListener(OnObjectChange);
    }
}
