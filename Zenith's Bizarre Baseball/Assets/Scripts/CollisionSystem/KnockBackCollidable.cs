using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class KnockBackCollidable : ICollidable
{
    [SerializeField] Float knockBackForce;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.velocity = (collider.transform.position - transform.position).normalized * knockBackForce.Value;
        }
    }
}