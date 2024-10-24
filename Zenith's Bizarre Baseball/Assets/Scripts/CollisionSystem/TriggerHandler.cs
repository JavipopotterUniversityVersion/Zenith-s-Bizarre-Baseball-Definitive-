using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CollidableType {ENTER, STAY, EXIT};

public class TriggerHandler : MonoBehaviour
{
    [SerializeField] List<CollidableGroup> OnEnterCollidables = new List<CollidableGroup>();
    public List<CollidableGroup> OnEnterCollidablesProp() => OnEnterCollidables;

    [SerializeField] List<CollidableGroup> OnStayCollidables;
    public List<CollidableGroup> OnStayCollidablesProp() => OnStayCollidables;
    [SerializeField] float checkDelay = 0.1f;
    float timer = 0;

    [SerializeField] List<CollidableGroup> OnExitCollidables;
    public List<CollidableGroup> OnExitCollidablesProp() => OnExitCollidables;

    bool canCollide = true;
    public void SetCanCollide(bool value) => canCollide = value;

    public void AddEnterCollidableGroup(CollidableGroup group, CollidableType type)
    {
        switch(type)
        {
            case CollidableType.ENTER:
                OnEnterCollidables.Add(group);
                break;
            case CollidableType.STAY:
                OnStayCollidables.Add(group);
                break;
            case CollidableType.EXIT:
                OnExitCollidables.Add(group);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canCollide) CollidableGroup.CheckCollidables(OnEnterCollidables, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(canCollide) CollidableGroup.CheckCollidables(OnExitCollidables, collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!canCollide) return;

        timer += Time.deltaTime;

        if(timer >= checkDelay)
        {
            timer = 0;
            CollidableGroup.CheckCollidables(OnStayCollidables, collision);
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

    public void SwapIdentifiable(Identifiable from, Identifiable to)
    {
        foreach(var collidableGroup in OnEnterCollidables)
        {
            for(int i = 0; i < collidableGroup.Targets.Length; i++)
            {
                if(collidableGroup.Targets[i] == from)
                {
                    collidableGroup.Targets[i] = to;
                }
            }
        }
    }
}

[Serializable]
public class CollidableGroup
{
    [SerializeField] Identifiable[] _targets;
    public Identifiable[] Targets => _targets;

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

    public bool CheckIdentifiables(Collider2D collider)
    {
        if(Targets.Length == 0) return true;

        Searchable type = collider.GetComponent<Searchable>();
        if(type != null)
        {
            foreach (Identifiable target in Targets)
            {
                if(type.IdentifiableType.DerivesFrom(target)) return true;
            }
        }

        return false;
    }

    public void SetLayer(LayerMask layer) => this.layer = layer;

    [SerializeField] ICollidable[] collidables;
    public ICollidable[] Collidables => collidables;

    public static void CheckCollidables(CollidableGroup[] collidables, Collider2D collision) => CheckCollidables(collidables.ToList(), collision);
    public static void CheckCollidables(List<CollidableGroup> collidables, Collider2D collision)
    {
        foreach (var collidableGroup in collidables)
        {
            if(collidableGroup.Layer == (collidableGroup.Layer | (1 << collision.gameObject.layer)) && collidableGroup.CheckConditions() 
            && collidableGroup.CheckIdentifiables(collision))
            {
                foreach (var collidable in collidableGroup.Collidables)
                {
                    collidable.OnCollide(collision);
                }
            }
        }
    }
}
