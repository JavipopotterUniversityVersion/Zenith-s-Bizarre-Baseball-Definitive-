using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] GameObject _prefab;
    
    public void ExecuteBehaviour()
    {
        ObjectPooler.Instance.SpawnFromPool(transform.position, transform.rotation, _prefab);
    }
}
