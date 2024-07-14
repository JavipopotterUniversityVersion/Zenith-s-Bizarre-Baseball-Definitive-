using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageChangerCollidable : ICollidable
{
    [SerializeField] Float _multiplier;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out DamageCollidable damageable))
        {
            damageable.SetMultiplier(_multiplier.Value);
        }
    }
}