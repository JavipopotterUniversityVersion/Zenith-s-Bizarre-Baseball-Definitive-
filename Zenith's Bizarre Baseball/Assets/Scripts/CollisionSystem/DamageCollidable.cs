using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollidable : ICollidable
{
    [SerializeField] float damage = 1;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out HealthHandler health))
        {
            health.TakeDamage(damage);
        }
    }

    public float GetDamage() => damage;
    public void SetDamage(float newDamage) => damage = newDamage;
}
