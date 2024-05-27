using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableICondition : ScriptableObject, ICondition
{
    public virtual bool CheckCondition() => true;
}

[Serializable]
public class ScriptableCondition
{
    [SerializeField] private ScriptableICondition conditionContainer;

    ScriptableICondition condition;
    public ScriptableICondition Cond => condition;

    [SerializeField] private bool negated;
    public bool Negated => negated;

    public static bool CheckAllConditions(ScriptableCondition[] condition)
    {
        if(condition.Length == 0) return true;
        
        foreach(ScriptableCondition cond in condition)
        {
            if(cond.Cond.CheckCondition() == cond.Negated)
            {
                return false;
            }
        }
        return true;
    }
}
