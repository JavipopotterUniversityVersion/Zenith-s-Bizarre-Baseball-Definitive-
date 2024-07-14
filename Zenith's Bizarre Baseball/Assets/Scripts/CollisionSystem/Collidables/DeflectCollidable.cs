using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectCollidable : ICollidable
{
    [SerializeField] Float _deflectMultiplier;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.velocity = Vector2.Reflect(rigidbody.velocity, (collider.transform.position - transform.position).normalized) * _deflectMultiplier.Value;
        }
    }
}