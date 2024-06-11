using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackCollidable : ICollidable
{
    [SerializeField] Float knockBackForce;
    [SerializeField] ObjectProcessor objectProcessor;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.velocity = (collider.transform.position - transform.position).normalized * objectProcessor.Result(knockBackForce.Value);
        }
        else if(collider.GetComponentInParent<Rigidbody2D>() != null)
        {
            Rigidbody2D parentRigidbody = collider.GetComponentInParent<Rigidbody2D>();
            parentRigidbody.velocity = (collider.transform.position - transform.position).normalized * objectProcessor.Result(knockBackForce.Value);
        }
    }
}