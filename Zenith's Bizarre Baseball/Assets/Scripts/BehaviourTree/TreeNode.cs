using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum TreeNodeState { SUCCESS, FAILURE, RUNNING }

public class TreeNode : MonoBehaviour
{
    protected TreeNodeState state;
    public virtual TreeNodeState Evaluate() => TreeNodeState.FAILURE;
}