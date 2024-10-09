using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : TreeNode
{
    public List<TreeNode> children = new List<TreeNode>();
    public override TreeNodeState Evaluate()
    {
        bool anyChildRunning = false;

        foreach (TreeNode child in children)
        {
            switch (child.Evaluate())
            {
                case TreeNodeState.FAILURE:
                    state = TreeNodeState.FAILURE;
                    return state;
                case TreeNodeState.SUCCESS:
                    continue;
                case TreeNodeState.RUNNING:
                    anyChildRunning = true;
                    break;
                default:
                    state = TreeNodeState.SUCCESS;
                    return state;
            }
        }

        state = anyChildRunning ? TreeNodeState.RUNNING : TreeNodeState.SUCCESS;
        return state;
    }

    private void OnValidate() {
        name = "Sequence";
    }
}