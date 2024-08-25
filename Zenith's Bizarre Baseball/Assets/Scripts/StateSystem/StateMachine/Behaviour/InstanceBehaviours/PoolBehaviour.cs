using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] bool _alignRotation = true;
    
    public void ExecuteBehaviour()
    {
        GameObject obj = ObjectPooler.Instance.SpawnFromPool(transform.position, Quaternion.identity, _prefab);
        if(_alignRotation) obj.transform.rotation = transform.rotation;
    }
}
