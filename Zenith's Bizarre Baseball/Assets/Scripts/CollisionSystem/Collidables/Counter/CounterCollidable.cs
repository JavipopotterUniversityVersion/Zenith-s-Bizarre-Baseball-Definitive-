using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class CounterCollidable : ICollidable
{
    [SerializeField] Float _speedAdder;

    [SerializeField] bool _hasMaxSpeed = false;
    
    [ConditionalField(nameof(_hasMaxSpeed), false)] 
    [SerializeField] float _maxSpeed = 0;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rigidbody) && collider.TryGetComponent(out Knockable knockable))
        {
            float force;

            if(rigidbody.velocity.magnitude < _speedAdder.Value * knockable.Reduction) force = _speedAdder.Value;
            else force = rigidbody.velocity.magnitude + (_speedAdder.Value * 0.5f);

            force = _hasMaxSpeed ? Mathf.Min(force, _maxSpeed) : force;
            knockable.Knock(force * (collider.transform.position - transform.position).normalized);
        }
    }
}