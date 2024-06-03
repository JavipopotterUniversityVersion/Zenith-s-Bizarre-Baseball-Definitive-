using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;

public class NodeGenerator : MonoBehaviour
{
    [SerializeField] Node _initialNode;
    
    [SerializeField] int _extension;
    public int Extension => _extension;

    [SerializeField] int maxBranchExtension;

    [SerializeField][Range(0, 1)] float _linearity = 0.5f;

    List<Node> _nodes = new List<Node>();
    public List<Node> Nodes => _nodes;

    UnityEvent onFinishedGeneration = new UnityEvent();
    public UnityEvent OnFinishedGeneration => onFinishedGeneration;

    [SerializeField] NodeSetting[] _nodeSettings;
    public NodeSetting[] NodeSettings => _nodeSettings;

    private void Start()
    {
        Node.extension = _extension;
        Node _currentNode = Instantiate(_initialNode, Vector3.zero, Quaternion.identity);
        Nodes.Add(_currentNode);

        GenerateNode(_currentNode);
    }

    async void GenerateNode(Node _currentNode)
    {
        await _currentNode.GenerateNodesTask(_linearity, this, maxBranchExtension);
        OnFinishedGeneration?.Invoke();
    }

    public bool CheckIntersecctions(Node node)
    {
        foreach (var n in _nodes)
        {
            foreach(Limit limit in n.Limits)
            {
                foreach(Limit limit2 in node.Limits)
                {
                    if(limit.Overlaps(limit2)) return true;
                }
            }
        }
        return false;
    }
}
