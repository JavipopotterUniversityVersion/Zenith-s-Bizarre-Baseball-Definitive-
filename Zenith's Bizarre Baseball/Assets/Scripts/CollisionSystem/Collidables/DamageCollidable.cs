using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollidable : ICollidable
{
    [SerializeField] Float Damage;
    [SerializeField] ObjectProcessor _processor;
    float multiplier = 1;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out HealthHandler health))
        {
            health.TakeDamage(_processor.Result(Damage.Value) * multiplier);
        }
    }

    public float GetMultiplier() => multiplier;
    public void SetDamage(string newDamage) => _processor.SetFormula(newDamage);
}
