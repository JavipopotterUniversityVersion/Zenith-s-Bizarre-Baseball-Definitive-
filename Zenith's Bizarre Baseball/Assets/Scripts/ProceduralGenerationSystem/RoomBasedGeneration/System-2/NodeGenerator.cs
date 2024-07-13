using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class NodeGenerator : MonoBehaviour
{
    [SerializeField] Node _initialNode;

    [SerializeField] int _extension;
    public int Extension => _extension;

    [SerializeField] int maxBranchExtension;

    [SerializeField][Range(0, 1)] float _linearity = 0.5f;

    List<Node> _nodes = new List<Node>();
    public List<Node> Nodes => _nodes;

    [SerializeField] UnityEvent onStartedGeneration = new UnityEvent();

    [SerializeField] UnityEvent onFinishedGeneration = new UnityEvent();
    public UnityEvent OnFinishedGeneration => onFinishedGeneration;

    [SerializeField] NodeSetting[] _nodeSettings;
    public NodeSetting[] NodeSettings => _nodeSettings;

    public List<GameObject> unusedNodes = new List<GameObject>();

    [ContextMenu("Start Generation")]
    private void Start()
    {
        onStartedGeneration.Invoke();
        Node.extension = _extension;
        Node _currentNode = Instantiate(_initialNode, Vector3.zero, Quaternion.identity);
        Nodes.Add(_currentNode);

        GenerateNode(_currentNode);
    }

    async void GenerateNode(Node _currentNode)
    {
        await _currentNode.GenerateNodesTask(_linearity, this, maxBranchExtension);
        await PlaceRequiredNodes();

        if(_nodes.Count < Extension) ScenesManager.ReloadSceneStatic();
        else
        {
            await Task.Delay(100);
            OnFinishedGeneration?.Invoke();
        }   
    }

    Task PlaceRequiredNodes()
    {
        foreach(NodeSetting setting in _nodeSettings)
        {
            bool failed = false;
            while(setting.AppearedRequiredTimes == false && !failed)
            {
                List<Node> _nodesWithRequiredExtension = 
                _nodes.Where(n => (float) n.ExtensionIndex / Node.largestBranch >= setting.MinExtension
                && (float) n.ExtensionIndex / Node.largestBranch <= setting.MaxExtension).ToList();

                Shuffle(_nodesWithRequiredExtension);

                if(_nodesWithRequiredExtension.Count == 0) 
                {
                    Debug.LogWarning("No nodes with the required extension");
                    _nodesWithRequiredExtension = _nodes;
                }

                int i = 0;

                while(i < _nodesWithRequiredExtension.Count && _nodesWithRequiredExtension[i].TryPlaceNode(setting.NodePrefab) == false) i++;

                if(i == _nodesWithRequiredExtension.Count) 
                {
                    failed = true;
                    _nodes.Clear();
                    Debug.LogWarning("Failed to place " + setting.NodePrefab.name + " in any node");
                }
                else
                {
                    setting.TimesAppeared++;
                }
            }
        }

        if(!(_nodes.Count < Extension)) DumpNodes();

        return Task.CompletedTask;
    }

    public void Shuffle<T>(IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    async void DumpNodes()
    {
        await Task.Delay(1000);
        foreach(GameObject node in unusedNodes)
        {
            Destroy(node);
        }
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
