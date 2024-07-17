using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollidable : ICollidable
{
    [SerializeField] ObjectProcessor maxCollisions;
    int _currentCollisions = 0;
    [SerializeField] GameObject _targetGameObject;

    private void Awake() 
    {
        if (_targetGameObject == null) _targetGameObject = gameObject;
    }

    public void SetMaxCollisions(string value) => maxCollisions.SetFormula(value);
    public void SetMaxCollisions(int value) => maxCollisions.SetFormula(value.ToString());

    public override void OnCollide(Collider2D collision)
    {
        _currentCollisions++;

        if (_currentCollisions >= maxCollisions.Result())
        {
            Destroy(_targetGameObject);
        }
    }
}
