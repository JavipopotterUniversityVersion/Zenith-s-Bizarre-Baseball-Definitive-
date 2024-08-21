using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageChangerCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _newDamage;
    public float NewDamage => _newDamage.Result();

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out DamageCollidable damageable))
        {
            damageable.SetDamage(NewDamage.ToString());
        }
    }
}