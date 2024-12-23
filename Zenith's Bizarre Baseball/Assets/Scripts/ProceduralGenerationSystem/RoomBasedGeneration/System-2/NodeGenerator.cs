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
    [SerializeField] RoomNode _initialNode;

    [SerializeField] int _extension;
    public int Extension => _extension;

    [SerializeField][Range(0, 1)] float _linearity = 0.5f;

    List<RoomNode> _nodes = new List<RoomNode>();
    public List<RoomNode> Nodes => _nodes;

    [SerializeField] Bool EntropyValue;
    int _visitedNodesCount = 0;
    public int VisitedNodesCount { get => _visitedNodesCount; set => _visitedNodesCount = value; }

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
        RoomNode.extension = _extension;
        RoomNode _currentNode = Instantiate(_initialNode, Vector3.zero, Quaternion.identity);
        Nodes.Add(_currentNode);

        GenerateNode(_currentNode);
    }

    private void OnDestroy() 
    {
        if(VisitedNodesCount < Nodes.Count) EntropyValue.SetValue(false);
    }

    async void GenerateNode(RoomNode _currentNode)
    {
        await _currentNode.GenerateNodesTask(_linearity, this, Extension);
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
                List<RoomNode> _nodesWithRequiredExtension = 
                _nodes.Where(n => (float) n.ExtensionIndex / RoomNode.largestBranch >= setting.MinExtension
                && (float) n.ExtensionIndex / RoomNode.largestBranch <= setting.MaxExtension).ToList();

                Shuffle(ref _nodesWithRequiredExtension);

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

    public void Shuffle<T>(ref List<T> list)  
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


    public bool CheckIntersecctions(RoomNode node)
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
