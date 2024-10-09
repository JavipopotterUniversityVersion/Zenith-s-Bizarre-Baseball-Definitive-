using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : TreeNode
{
    public List<TreeNode> children = new List<TreeNode>();
    public override TreeNodeState Evaluate()
    {
        foreach (TreeNode child in children)
        {
            switch (child.Evaluate())
            {
                case TreeNodeState.SUCCESS:
                    state = TreeNodeState.SUCCESS;
                    return state;
                case TreeNodeState.FAILURE:
                    continue;
                case TreeNodeState.RUNNING:
                    state = TreeNodeState.RUNNING;
                    return state;
                default:
                    continue;
            }
        }

        state = TreeNodeState.FAILURE;
        return state;
    }

    private void OnValidate() {
        name = "Selector";
    }
}