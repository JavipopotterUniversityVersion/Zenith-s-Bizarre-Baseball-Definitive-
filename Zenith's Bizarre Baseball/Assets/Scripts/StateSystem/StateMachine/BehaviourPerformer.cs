using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BehaviourPerformer
{
    [SerializeField] Condition[] condition;
    [SerializeField] GameObject[] behaviourContainers;
    IBehaviour[] behaviours;

    bool initialized = false;

    public bool CheckConditions() => Condition.CheckAllConditions(condition);
    public void ExecuteBehaviours()
    {
        Initialize();

        foreach(IBehaviour behaviour in behaviours)
        {
            behaviour.ExecuteBehaviour();
        }
    }

    public void Initialize()
    {
        if(!initialized)
        {
            foreach(Condition cond in condition)
            {
                cond.Initialize();
            }

            behaviours = new IBehaviour[behaviourContainers.Length];
            for(int i = 0; i < behaviourContainers.Length; i++)
            {
                behaviours[i] = behaviourContainers[i].GetComponent<IBehaviour>();
            }

            initialized = true;
        }
    }

    public bool Perform()
    {
        Initialize();

        if(Condition.CheckAllConditions(condition))
        {
            ExecuteBehaviours();
            return true;
        }
        
        return false;
    }

    public static void Perform(BehaviourPerformer[] performers)
    {
        foreach(BehaviourPerformer performer in performers)
        {
            performer.Perform();
        }
    }
}

[System.Serializable]
public class Condition
{
    [SerializeField] private GameObject conditionContainer;

    ICondition condition;
    public ICondition Cond => condition;

    [SerializeField] private bool negated;
    public bool Negated => negated;

    public void Initialize()
    {
        if(conditionContainer.TryGetComponent(out ICondition cond))
        {
            condition = cond;
        }
        else
        {
            Debug.LogError("Womp Womp Condition container does not contain a condition component");
        }
    }

    public static void InitializeAll(Condition[] conditions)
    {
        foreach(Condition cond in conditions)
        {
            cond.Initialize();
        }
    }

    public static bool CheckAllConditions(Condition[] condition)
    {
        if(condition.Length == 0) return true;
        
        foreach(Condition cond in condition)
        {
            if(cond.Cond.CheckCondition() == cond.Negated)
            {
                return false;
            }
        }
        return true;
    }

    public static float Value(Condition[] conditions) => CheckAllConditions(conditions) ? 1 : 0;
}
