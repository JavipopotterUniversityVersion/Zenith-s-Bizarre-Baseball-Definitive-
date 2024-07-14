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
            float force;

            if(rigidbody.velocity.magnitude < _speedAdder.Value) force = _speedAdder.Value;
            else force = rigidbody.velocity.magnitude + (_speedAdder.Value * 0.5f);

            rigidbody.velocity = force * (collider.transform.position - transform.position).normalized;
        }
    }
}