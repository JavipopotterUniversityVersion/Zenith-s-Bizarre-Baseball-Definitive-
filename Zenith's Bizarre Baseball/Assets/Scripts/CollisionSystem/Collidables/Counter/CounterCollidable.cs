using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class CounterCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _speedToAdd;

    [SerializeField] bool _hasMaxSpeed = false;
    
    [ConditionalField(nameof(_hasMaxSpeed), false)] 
    [SerializeField] float _maxSpeed = 0;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rigidbody) && collider.TryGetComponent(out Knockable knockable))
        {
            float force;

            if(rigidbody.velocity.magnitude < _speedToAdd.Result() * knockable.Reduction) force = _speedToAdd.Result();
            else force = rigidbody.velocity.magnitude + (_speedToAdd.Result() * 0.5f);

            force = _hasMaxSpeed ? Mathf.Min(force, _maxSpeed) : force;
            knockable.Knock(force * (collider.transform.position - transform.position).normalized);
        }
    }
}