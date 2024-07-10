using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _healAmount;
    public override void OnCollide(Collider2D other)
    {
        if (other.TryGetComponent(out HealthHandler healthHandler))
        {
            healthHandler.Heal(_healAmount.Result());
        }
    }
}
