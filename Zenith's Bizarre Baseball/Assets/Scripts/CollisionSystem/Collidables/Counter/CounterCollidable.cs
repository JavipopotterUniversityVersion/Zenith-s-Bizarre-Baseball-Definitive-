using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class CounterCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _speedToAdd;
    [SerializeField] AnimationCurve _speedCurve;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rb) && collider.TryGetComponent(out Knockable knockable))
        {
            float force = _speedToAdd.Result();

            float rbSpeed = rb.velocity.magnitude;
            force = force * _speedCurve.Evaluate(rbSpeed/force) * knockable.Reduction;

            rb.velocity = (rbSpeed + force) * (collider.transform.position - transform.position).normalized;
        }
    }
}