using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSeterCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _speedProcessor;
    public override void OnCollide(Collider2D collider)
    {
        if(collider.TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = rb.linearVelocity.normalized * _speedProcessor.Result(rb.linearVelocity.magnitude);
        }
    }
}
