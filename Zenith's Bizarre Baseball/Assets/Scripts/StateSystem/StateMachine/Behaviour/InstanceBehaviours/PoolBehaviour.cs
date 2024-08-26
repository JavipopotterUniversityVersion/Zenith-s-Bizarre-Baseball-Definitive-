using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] bool _alignRotation = true;
    bool disabled = false;
    
    private void Awake() {
        if(ObjectPooler.Instance == null)
        {
            disabled = true;
            Debug.LogWarning("ObjectPooler is not initialized. Disabling PoolBehaviour.");
        }
    }

    public void ExecuteBehaviour()
    {
        if(disabled) return;
        
        GameObject obj = ObjectPooler.Instance.SpawnFromPool(transform.position, Quaternion.identity, _prefab);
        if(_alignRotation) obj.transform.rotation = transform.rotation;
    }
}
