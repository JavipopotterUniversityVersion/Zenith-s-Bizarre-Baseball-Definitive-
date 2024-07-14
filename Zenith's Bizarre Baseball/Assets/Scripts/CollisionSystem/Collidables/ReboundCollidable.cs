using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReboundCollidable : ICollidable
{
    [SerializeField] Float _reboundMultiplier;
    Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    public override void OnCollide(Collider2D collider)
    {
        rb.velocity = Vector2.Reflect(rb.velocity, (collider.transform.position - transform.position).normalized) * _reboundMultiplier.Value;
    }
}