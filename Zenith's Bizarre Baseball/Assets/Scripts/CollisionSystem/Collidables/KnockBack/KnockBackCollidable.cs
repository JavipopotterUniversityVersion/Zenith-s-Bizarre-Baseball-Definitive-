using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _knockBackForce;
    public float KnockBackForce => _knockBackForce.Result();

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Knockable knockable))
        {
            knockable.Knock(KnockBackForce * (collider.transform.position - transform.position).normalized);
        }
    }
}