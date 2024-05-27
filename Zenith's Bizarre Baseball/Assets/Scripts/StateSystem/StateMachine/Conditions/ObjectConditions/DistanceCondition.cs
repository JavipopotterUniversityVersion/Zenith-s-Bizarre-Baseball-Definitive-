using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCondition : MonoBehaviour, ICondition
{
    TargetHandler targetHandler;
    [SerializeField] Vector2 range;

    private void Awake()
    {
        targetHandler = GetComponentInParent<TargetHandler>();
    }

    public bool CheckCondition()
    {
        float distance = 0;
       if(targetHandler != null) distance = Vector2.Distance(transform.position, targetHandler.target.position); // si no entra en el if, interpretamos que el valor nos da igual
       // Debug.Log(distance <= range.x && distance <= range.y);
        bool condicion = distance >= range.x && distance <= range.y;
       // Debug.Log(condicion);
        return condicion;
    }

    private void OnValidate() => gameObject.name = $"distance({range.x}, {range.y})";
}
