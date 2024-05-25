using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageChangerCollidable : ICollidable
{
    [SerializeField] float _multiplier = 1;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out DamageCollidable damageable))
        {
            damageable.SetMultiplier(_multiplier);
        }
    }
}