using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterCollidable : ICollidable
{
    [SerializeField] float _counterMultiplier = 1f;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.velocity =  rigidbody.velocity.magnitude * _counterMultiplier * (collider.transform.position - transform.position).normalized;
        }
    }
}