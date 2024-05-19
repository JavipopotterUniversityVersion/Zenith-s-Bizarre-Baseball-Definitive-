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

    public bool Perform()
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

        if(Condition.CheckAllConditions(condition))
        {
            foreach(IBehaviour behaviour in behaviours)
            {
                behaviour.ExecuteBehaviour();
            }
        }
        
        return Condition.CheckAllConditions(condition);
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
        condition = conditionContainer.GetComponent<ICondition>();
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
}
