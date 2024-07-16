using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollidable : ICollidable
{
    [SerializeField] ObjectProcessor maxCollisions;
    int requiredCollisions = 1;
    [SerializeField] GameObject _targetGameObject;

    private void Awake() 
    {
        if (_targetGameObject == null) _targetGameObject = gameObject;
    }

    private void Start() => requiredCollisions = (int)maxCollisions.Result();
    public void SetMaxCollisions(string value) => maxCollisions.SetFormula(value);
    public void SetMaxCollisions(int value) => maxCollisions.SetFormula(value.ToString());

    public override void OnCollide(Collider2D collision)
    {
        requiredCollisions--;

        if (requiredCollisions <= 0)
        {
            Destroy(_targetGameObject);
        }
    }
}
