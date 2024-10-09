using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTask : TreeNode
{
    [SerializeField] private Condition _failureCondition;
    [SerializeField] private Condition _successCondition;
    [SerializeField] UnityEvent action;
    bool _actionInvoked = false;

    private void Awake() {
        _failureCondition.Initialize();
        _successCondition.Initialize();
    }

    public override TreeNodeState Evaluate()
    {
        if(_failureCondition.Cond.CheckCondition())
        {
            _actionInvoked = false;
            return TreeNodeState.SUCCESS;
        }
        else if(_successCondition.Cond.CheckCondition())
        {
            _actionInvoked = false;
            return TreeNodeState.FAILURE;
        }
        else
        {
            if(!_actionInvoked)
            {
                action.Invoke();
                _actionInvoked = true;
            }

            return TreeNodeState.RUNNING;
        }
    }
}