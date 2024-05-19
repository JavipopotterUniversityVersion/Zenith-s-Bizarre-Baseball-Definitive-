using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSwapperCollidable : ICollidable
{
    [SerializeField] SwapRelation[] swapRelation;
    
    public override void OnCollide(Collider2D collision)
    {
        if(collision.TryGetComponent(out TriggerHandler triggerHandler))
        {
            triggerHandler.SwapLayers(swapRelation);
        }
    }
}

[System.Serializable]
public class SwapRelation
{
    public LayerMask FromLayer;
    public LayerMask ToLayer;
}