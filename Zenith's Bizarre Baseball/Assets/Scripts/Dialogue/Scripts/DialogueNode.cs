using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class DialogueNode : Node
{
    public string GUID;
    public bool EntryPoint = false;
    public NodeType NodeType = NodeType.Dialogue;
}

public enum NodeType
{
    Dialogue,
    Choice,
    Label,
    LabelJump,
    Conditional,
    Background
}