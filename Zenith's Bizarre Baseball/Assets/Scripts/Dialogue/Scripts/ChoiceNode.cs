using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class ChoiceNode : Node
{
    string _guid;
    public string GUID { get => _guid; set => _guid = value; }
}