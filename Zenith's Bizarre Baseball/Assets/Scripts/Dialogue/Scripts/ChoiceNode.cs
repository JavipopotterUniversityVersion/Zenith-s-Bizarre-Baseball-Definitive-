#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;

public class ChoiceNode : Node
{
    string _guid;
    public string GUID { get => _guid; set => _guid = value; }
}

#endif