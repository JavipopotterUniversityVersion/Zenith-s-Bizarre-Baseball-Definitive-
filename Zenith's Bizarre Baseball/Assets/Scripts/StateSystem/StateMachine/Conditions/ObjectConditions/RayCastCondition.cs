using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastCondition : MonoBehaviour, ICondition
{
    [SerializeField] float length = 10;
    [SerializeField] LayerMask targetLayer;

    public bool CheckCondition()
    {
        return Physics2D.Raycast(transform.position, transform.up, length, targetLayer);
    }
}
