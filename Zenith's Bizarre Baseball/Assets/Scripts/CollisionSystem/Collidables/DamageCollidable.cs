using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _damage;
    public float Damage => _damage.Result();

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out HealthHandler health))
        {
            health.TakeDamage(Damage);
        }
    }

    public void SetDamage(string newDamage) => _damage.SetFormula(newDamage);
}
