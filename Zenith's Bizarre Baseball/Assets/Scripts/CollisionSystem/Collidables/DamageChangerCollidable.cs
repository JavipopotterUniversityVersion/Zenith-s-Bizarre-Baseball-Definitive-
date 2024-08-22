using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageChangerCollidable : ICollidable
{
    [SerializeField] Float _value;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out DamageCollidable damageable))
        {
            damageable.SetDamage(_value.Value.ToString());
        }
    }
}