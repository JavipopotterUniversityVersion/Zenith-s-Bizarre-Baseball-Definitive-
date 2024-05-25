using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollidable : ICollidable
{
    [SerializeField] Int maxCollisions;
    int requiredCollisions = 1;

    private void Start() => requiredCollisions = maxCollisions.Value;

    public override void OnCollide(Collider2D collision)
    {
        requiredCollisions--;

        if (requiredCollisions <= 0) Destroy(gameObject);
    }
}
