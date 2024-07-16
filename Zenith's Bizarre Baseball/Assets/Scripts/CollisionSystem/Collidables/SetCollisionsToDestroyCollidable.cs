using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCollisionsToDestroyCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _collisionsToDestroy;
    public override void OnCollide(Collider2D collider)
    {
        if(collider.TryGetComponent(out DestroyCollidable destroyCollidable))
        {
            destroyCollidable.SetMaxCollisions((int)_collisionsToDestroy.Result());
        }
    }
}
