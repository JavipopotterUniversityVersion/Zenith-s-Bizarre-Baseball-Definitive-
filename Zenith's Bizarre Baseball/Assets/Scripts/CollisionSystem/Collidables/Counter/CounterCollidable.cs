using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterCollidable : ICollidable
{
    [SerializeField] Float _speedAdder;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rigidbody) && collider.TryGetComponent(out Knockable knockable))
        {
            float force;

            if(rigidbody.velocity.magnitude < _speedAdder.Value * knockable.Reduction) force = _speedAdder.Value;
            else force = rigidbody.velocity.magnitude + (_speedAdder.Value * 0.5f);

            knockable.Knock(force * (collider.transform.position - transform.position).normalized);
        }
    }
}