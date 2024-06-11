using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSwapperCollidable : ICollidable, IGameObjectProcessor
{
    [SerializeField] SwapRelation[] swapRelation;
    
    public override void OnCollide(Collider2D collision) => Process(collision.gameObject);

    public void Process(GameObject gameObject)
    {
        if(gameObject.TryGetComponent(out TriggerHandler triggerHandler))
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

public interface IGameObjectProcessor
{
    public void Process(GameObject gameObject);
}