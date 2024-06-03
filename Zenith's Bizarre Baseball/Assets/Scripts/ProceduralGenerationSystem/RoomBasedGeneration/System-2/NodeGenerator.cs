using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;
using System.Linq;

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
        await PlaceRequiredNodes();
        await Task.Delay(1000);
        OnFinishedGeneration?.Invoke();
    }

    Task PlaceRequiredNodes()
    {
        foreach(NodeSetting setting in _nodeSettings)
        {
            while(setting.AppearedRequiredTimes == false)
            {
                List<Node> _nodesWithMinExtension = _nodes.Where(n => n.ExtensionIndex >= setting.MinExtension).ToList();

                Node randomNode = _nodesWithMinExtension[Random.Range(0, _nodesWithMinExtension.Count)];

                int i = 0;
                while(randomNode.TryPlaceNode(setting.NodePrefab) == false && i < 10)
                {
                    randomNode = _nodesWithMinExtension[Random.Range(0, _nodesWithMinExtension.Count)];
                    i++;
                }

                if(i >= 10) Debug.LogError("Could not place the required node " + setting.NodePrefab.name);
                else setting.TimesAppeared++;
            }
        }

        return Task.CompletedTask;
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
