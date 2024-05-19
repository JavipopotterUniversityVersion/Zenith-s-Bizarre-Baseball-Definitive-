using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollidable : ICollidable
{
    [SerializeField] int requiredCollisions = 1;
    public override void OnCollide(Collider2D collision)
    {
        requiredCollisions--;

        if (requiredCollisions <= 0) Destroy(gameObject);
    }
}
