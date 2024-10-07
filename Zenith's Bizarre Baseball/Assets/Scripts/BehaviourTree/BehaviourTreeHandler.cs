using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

public enum ChildStatus { SUCCESS, FAILURE, RUNNING }   
public class BehaviourTreeHandler : MonoBehaviour
{
    [SerializeReference] public Leaf root;
    
}

public interface Leaf
{
    IEnumerator Run(ref ChildStatus status);
}

// [Serializable]
// public class SelectorLeaf : Leaf
// {
//     public Leaf[] children;
// }

// [Serializable]
// public class SequenceLeaf : Leaf
// {
// }

// [Serializable]
// public class TaskLeaf : Leaf
// {
//     public ObjectProcessor condition;
//     public UnityEvent action;
// }

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(BehaviourTreeHandler))]
public class BehaviourTreeCustomEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif