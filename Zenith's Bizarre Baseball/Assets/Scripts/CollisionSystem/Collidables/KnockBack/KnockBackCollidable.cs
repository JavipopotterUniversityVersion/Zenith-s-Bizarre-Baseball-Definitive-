using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackCollidable : ICollidable
{
    [SerializeField] Float knockBackForce;
    [SerializeField] ObjectProcessor objectProcessor;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Knockable knockable))
        {
            knockable.Knock(knockBackForce.Value * objectProcessor.Result() * (collider.transform.position - transform.position).normalized);
        }
    }
}