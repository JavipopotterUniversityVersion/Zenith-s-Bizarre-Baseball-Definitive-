using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    [SerializeField] CollidableGroup[] OnEnterCollidables;
    public CollidableGroup[] OnEnterCollidablesProp() => OnEnterCollidables;

    [SerializeField] CollidableGroup[] OnExitCollidables;
    public CollidableGroup[] OnExitCollidablesProp() => OnExitCollidables;

    private void OnTriggerEnter2D(Collider2D collision) => CheckCollidables(OnEnterCollidables, collision);
    private void OnTriggerExit2D(Collider2D collision) => CheckCollidables(OnExitCollidables, collision);

    void CheckCollidables(CollidableGroup[] collidables, Collider2D collision)
    {
        foreach (var collidableGroup in collidables)
        {
            if(collidableGroup.Layer == (collidableGroup.Layer | (1 << collision.gameObject.layer)) && collidableGroup.CheckConditions())
            {
                foreach (var collidable in collidableGroup.Collidables)
                {
                    collidable.OnCollide(collision);
                }
            }
        }
    }

    public void SwapLayers(SwapRelation[] swapRelations)
    {
        foreach(var collidableGroup in OnEnterCollidables)
        {
            foreach(var swap in swapRelations)
            {
                if(collidableGroup.Layer == (collidableGroup.Layer | swap.FromLayer))
                {
                    collidableGroup.SetLayer(swap.ToLayer);
                }
            }
        }
    }
}

[Serializable]
public class CollidableGroup
{
    [SerializeField] LayerMask layer;
    public LayerMask Layer => layer;

    [SerializeField] Condition[] conditions;
    public bool CheckConditions() => Condition.CheckAllConditions(conditions);

    public void SetLayer(LayerMask layer) => this.layer = layer;

    [SerializeField] ICollidable[] collidables;
    public ICollidable[] Collidables => collidables;
}
