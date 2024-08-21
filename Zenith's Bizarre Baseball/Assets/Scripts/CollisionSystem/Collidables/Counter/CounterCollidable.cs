using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class CounterCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _speedAdder;
    public float SpeedAdder => _speedAdder.Result();

    [SerializeField] bool _hasMaxSpeed = false;

    [ConditionalField(nameof(_hasMaxSpeed), false)] 
    [SerializeField] float _maxSpeed = 0;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rigidbody) && collider.TryGetComponent(out Knockable knockable))
        {
            float force;

            if(rigidbody.velocity.magnitude < SpeedAdder * knockable.Reduction) force = SpeedAdder;
            else force = rigidbody.velocity.magnitude + (SpeedAdder * 0.5f);

            force = _hasMaxSpeed ? Mathf.Min(force, _maxSpeed) : force;
            knockable.Knock(force * (collider.transform.position - transform.position).normalized);
        }
    }
}