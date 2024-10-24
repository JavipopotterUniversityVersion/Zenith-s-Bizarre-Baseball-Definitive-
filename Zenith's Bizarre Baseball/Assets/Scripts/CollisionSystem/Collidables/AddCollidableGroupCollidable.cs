using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCollidableGroupCollidable : ICollidable
{
    [SerializeField] CollidableGroup collidableGroup;
    [SerializeField] CollidableType type;
    public override void OnCollide(Collider2D collider)
    {
        if(collider.TryGetComponent(out TriggerHandler handler))
        {
            handler.AddEnterCollidableGroup(collidableGroup, type);
        }
    }
}
