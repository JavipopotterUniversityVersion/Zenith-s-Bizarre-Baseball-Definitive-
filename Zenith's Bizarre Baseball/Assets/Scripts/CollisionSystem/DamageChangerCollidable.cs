using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageChangerCollidable : ICollidable
{
    [SerializeField] bool _isMultiplier = true;
    [SerializeField] float _damage = 1;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out DamageCollidable damageable))
        {
            damageable.SetDamage(_isMultiplier ? damageable.GetDamage() * _damage : _damage);
        }
    }
}