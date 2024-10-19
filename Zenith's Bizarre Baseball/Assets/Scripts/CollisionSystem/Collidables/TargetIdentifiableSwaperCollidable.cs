using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIdentifiableSwaperCollidable : ICollidable
{
    [SerializeField] Identifiable _from;
    [SerializeField] Identifiable _to;

    public override void OnCollide(Collider2D collider)
    {
        if(collider.TryGetComponent(out TriggerHandler triggerHandler))
        {
            triggerHandler.SwapIdentifiable(_from, _to);
        }
    }
}
