using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollidable : ICollidable
{
    [SerializeField] Float Damage;
    float multiplier = 1;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out HealthHandler health))
        {
            health.TakeDamage(Damage.Value * multiplier);
        }
    }

    public float GetMultiplier() => multiplier;
    public void SetMultiplier(float newDamage) => multiplier = newDamage;
}
