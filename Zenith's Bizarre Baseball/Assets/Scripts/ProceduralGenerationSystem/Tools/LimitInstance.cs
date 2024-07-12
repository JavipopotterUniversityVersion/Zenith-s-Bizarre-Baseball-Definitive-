using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitInstance : MonoBehaviour
{
    [SerializeField] Limit _instanceLimit;
    public Limit InstanceLimit => _instanceLimit;

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(InstanceLimit.GetCenter(), InstanceLimit.GetSize());
    }
}
