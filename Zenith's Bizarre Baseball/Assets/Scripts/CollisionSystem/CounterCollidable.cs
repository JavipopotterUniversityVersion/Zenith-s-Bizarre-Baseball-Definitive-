using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterCollidable : ICollidable
{
    [SerializeField] Float _speedAdder;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.velocity =  (rigidbody.velocity.magnitude + _speedAdder.Value) * (collider.transform.position - transform.position).normalized;
        }
    }
}