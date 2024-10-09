using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTree
{
    public class Tree : MonoBehaviour, IBehaviour
    {
        [SerializeField] private TreeNode _root = null;
        public TreeNode Root { get => _root; set => _root = value; }

        public void ExecuteBehaviour()
        {
            _root.Evaluate();
        }
    }
}