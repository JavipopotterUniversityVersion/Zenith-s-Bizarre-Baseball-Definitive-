using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    [SerializeField] CollidableGroup[] OnEnterCollidables;
    public CollidableGroup[] OnEnterCollidablesProp() => OnEnterCollidables;

    [SerializeField] CollidableGroup[] OnStayCollidables;
    public CollidableGroup[] OnStayCollidablesProp() => OnStayCollidables;
    [SerializeField] float checkDelay = 0.1f;
    float timer = 0;

    [SerializeField] CollidableGroup[] OnExitCollidables;
    public CollidableGroup[] OnExitCollidablesProp() => OnExitCollidables;

    bool canCollide = true;
    public void SetCanCollide(bool value) => canCollide = value;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canCollide) CheckCollidables(OnEnterCollidables, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(canCollide) CheckCollidables(OnExitCollidables, collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!canCollide) return;

        timer += Time.deltaTime;

        if(timer >= checkDelay)
        {
            timer = 0;
            CheckCollidables(OnStayCollidables, collision);
        }
    }

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

    bool initialized = false;
    public bool CheckConditions()
    {
        if(!initialized)
        {
            initialized = true;
            Condition.InitializeAll(conditions);
        }

        return Condition.CheckAllConditions(conditions);
    }

    public void SetLayer(LayerMask layer) => this.layer = layer;

    [SerializeField] ICollidable[] collidables;
    public ICollidable[] Collidables => collidables;
}
