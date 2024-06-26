using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System.Linq;

public class Node : MonoBehaviour
{
    NodeGenerator _generator;
    public NodeGenerator Generator {set => _generator = value; get => _generator;}
    [SerializeField] Gate[] _gates;
    public Gate[] Gates => _gates;

    [SerializeField] bool ignoreRoomExtension = false;
    public bool IgnoreRoomExtension => ignoreRoomExtension;

    [SerializeField] List<NodeSetting> nodeSettings = new List<NodeSetting>();
    [SerializeField] RoomAccess _access;
    public RoomAccess Access
    {
        get => _access;
        set
        {
            _access = value;
        }
    }

    float _linearity = 0.5f;

    public static int extension = 10;
    public static int largestBranch = 0;

    int extensionIndex = 0;
    public int ExtensionIndex => extensionIndex;

    string[] names = new string[] {"Pepe", "Juan", "Pedro", "Luis", "Carlos", "Jorge", "Ricardo", "Miguel", "Alberto", "Fernando", "Rob", "John", "Mike", "Steve", "Tom", "Jerry", "Rick", "Morty", "Beth", "Summer", "Jerry", "Birdperson", "Tammy", "Squanchy", "Unity", "Mr. Poopybutthole", "Noob Noob", "Scary Terry", "Abradolf Lincler", "Pencilvester", "Photography Raptor", "Crocubot", "Gearhead", "Million Ants", "Trunk People", "Gazorpazorpfield", "Ants in my Eyes Johnson", "Reverse Giraffe", "Hamurai", "Amish Cyborg", "Purge Planet Ruler", "Cromulon", "Gromflomite", "Plutonian", "Zigerion", "Meeseeks", "Cronenberg", "Fart", "Giant Head", "Giant Testicle Monster", "Giant Arm", "Giant Cat", "Giant Beetle", "Giant Spider"};

    private void Awake() {
        name = names[UnityEngine.Random.Range(0, names.Length)];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        foreach(Limit limit in _limits)
        {
            Gizmos.DrawWireCube(limit.GetCenter(), limit.GetSize());
        }
    }

    private void OnValidate() {
        if(_gates.Length == 0)
        {
            DoorIdentifier[] doors = GetComponentsInChildren<DoorIdentifier>();
            _gates = new Gate[doors.Length];

            for (int i = 0; i < doors.Length; i++)
            {
                _gates[i] = new Gate(doors[i].transform, doors[i].SetPositionAccess());
            }
        }
    }

    public void SetAccess(RoomAccess access) => Access = access;
    RoomAccess ReturnRandomAccess(RoomAccess accessValue)
    {
        if(UnityEngine.Random.value < _linearity) return accessValue | GetOppositeAccess(accessValue);

        RoomAccess newAccess = (RoomAccess) UnityEngine.Random.Range(1, 15);
        while (accessValue == newAccess)
        {           
            newAccess = (RoomAccess) UnityEngine.Random.Range(1, 15);
        }
        accessValue |= newAccess;
        return accessValue;
    }

    public async Task GenerateNodesTask(float linearity, NodeGenerator generator, int branchExtension)
    {
        GenerateNodes(linearity, generator, branchExtension);
        await Task.Delay(8000);
    }

    public async void GenerateNodes(float linearity, NodeGenerator generator, int branchExtension)
    {
        _generator = generator;
        _linearity = linearity;

        nodeSettings.AddRange(generator.NodeSettings.ToList());
        extensionIndex = _generator.Extension - branchExtension;
        if(extensionIndex > largestBranch) largestBranch = extensionIndex;

        for (int i = 0; i < _gates.Length; i++)
        {
            if(extension <= 0 || branchExtension <= 0)
            {
                if(ignoreRoomExtension)
                {
                    nodeSettings.RemoveAll(setting => setting.NodePrefab.GetComponent<Node>().IgnoreRoomExtension);
                }
                else
                {
                    CloseNodes();
                    return;
                }
            }
            
            Gate gate = _gates[i];

            if (gate.IsConnected || !Access.HasFlag(gate.Access)) continue;

            List<NodeSetting> possibleNodes = nodeSettings;
            possibleNodes.AddRange(gate.possibleNodes);

            GameObject nodePrefab = NodeSetting.RandomNodeSetting(possibleNodes);
            if (nodePrefab == null)
            {
                CloseNodes();
                return;
            }

            Node node = Instantiate(nodePrefab).GetComponent<Node>();
            node.Generator = _generator;

            int j = 0;

            while (!node.ConnectNodes(ref gate))
            {
                if(Application.isEditor) DestroyImmediate(node.gameObject, false);
                else Destroy(node.gameObject);

                node = Instantiate(NodeSetting.RandomNodeSetting(nodeSettings)).GetComponent<Node>();
                node.Generator = _generator;

                j++;

                if (j > 10)
                {
                    Destroy(node.gameObject);
                    CloseAccess(gate.Access);
                    break;
                }
            }

            if (j > 10) continue;

            _generator.Nodes.Add(node);
            node.SetAccess(ReturnRandomAccess(GetOppositeAccess(gate.Access)));
            extension--;

            await Task.Delay(100/(branchExtension+1));
            node.GenerateNodes(_linearity, _generator, branchExtension - 1);
        }
    }

    void CloseNodes()
    {
        foreach(Gate gate in _gates)
        {
            if(!gate.IsConnected) CloseAccess(gate.Access);
        }
    }

    public bool TryPlaceNode(GameObject nodePrefab)
    {
        Gate gate = OpenFirstAvailableGate();
        if(gate == null) return false;

        Node node = Instantiate(nodePrefab).GetComponent<Node>();
        node.Generator = _generator;

        bool result = node.ConnectNodes(ref gate);

        if(!result)
        {
            Destroy(node.gameObject);
            CloseAccess(gate.Access);
        }
        else
        {
            node.SetAccess(GetOppositeAccess(gate.Access));
            _generator.Nodes.Add(node);
        }

        return result;
    }

    Gate OpenFirstAvailableGate()
    {
        Gate gate = null;

        int i = 0;
        while(i < _gates.Length && gate == null)
        {
            Gate gateToCheck = _gates[i];
            if (!gateToCheck.IsConnected)
            { 
                gate = gateToCheck;
                OpenAccess(gate.Access);
            }

            i++;
        }

        return gate;
    }
   
    void CloseAccess(RoomAccess access) => Access &= ~access;
    void OpenAccess(RoomAccess access) => Access |= access;

    bool CanPlaceNode()
    {
        return !_generator.CheckIntersecctions(this);
    }


    [SerializeField] Limit[] _limits;
    public Limit[] Limits => _limits;

    public bool ConnectNodes(ref Gate gate)
    {
        foreach(Gate g in _gates)
        {
            if(GetOppositeAccess(g.Access) == gate.Access)
            {
                g.ConnectedGate = gate;
                gate.ConnectedGate = g;
                Vector3 nodePosition = gate.Transform.position + (transform.position - g.Transform.position);
                transform.position = nodePosition;
                break;
            }
        }

        return CanPlaceNode();
    }

    RoomAccess GetOppositeAccess(RoomAccess access)
    {
        switch (access)
        {
            case RoomAccess.North:
                return RoomAccess.South;
            case RoomAccess.East:
                return RoomAccess.West;
            case RoomAccess.South:
                return RoomAccess.North;
            case RoomAccess.West:
                return RoomAccess.East;
            default:
                return RoomAccess.None;
        }
    }
}

[Serializable]
public class Gate
{
    [SerializeField] Transform _transform;
    public Transform Transform => _transform;

    public NodeSetting[] possibleNodes;

    public bool IsConnected => _connectedGate != null;

    [SerializeField] Gate _connectedGate;
    public Gate ConnectedGate
    {
        get => _connectedGate;
        set => _connectedGate = value;
    }

    [SerializeField] RoomAccess _access;
    public RoomAccess Access => _access;

    public Gate(Transform transform, RoomAccess access)
    {
        _transform = transform;
        _access = access;
    }
}

[Serializable]
public class NodeSetting
{
    [SerializeField] GameObject _nodePrefab;
    public GameObject NodePrefab => _nodePrefab;

    [SerializeField] [Range(0, 1)]
    float _probability;

    public float Probability => _probability;

    [SerializeField] int _minNumberOfNodes;
    public int MinNumberOfNodes => _minNumberOfNodes;

    int _timesAppeared;
    public int TimesAppeared
    {
        get => _timesAppeared;
        set => _timesAppeared = value;
    }

    [SerializeField] int _maxNumberOfNodes;
    public int MaxNumberOfNodes => _maxNumberOfNodes;

    public bool AppearedRequiredTimes => _timesAppeared >= _minNumberOfNodes;

    [SerializeField] [Range(0,1)] float minExtension;
    public float MinExtension => minExtension;

    [SerializeField] [Range(0,1)] float maxExtension;
    public float MaxExtension => maxExtension;

    public static GameObject RandomNodeSetting(List<NodeSetting> nodeSettings) => RandomNodeSetting(nodeSettings.ToArray());

    public static GameObject RandomNodeSetting(NodeSetting[] nodeSettings)
    {
        if(nodeSettings.Length == 0) return null;

        float totalProbability = 0;
        foreach(NodeSetting setting in nodeSettings)
        {
            totalProbability += setting.Probability;
        }

        float randomValue = UnityEngine.Random.Range(0, totalProbability);
        float currentProbability = 0;

        foreach(NodeSetting setting in nodeSettings)
        {
            currentProbability += setting.Probability;
            if(setting._maxNumberOfNodes > 0 && setting.TimesAppeared >= setting.MaxNumberOfNodes) continue;
            if(randomValue <= currentProbability)
            {
                setting._timesAppeared++;
                return setting._nodePrefab;
            }
        }

        return null;
    }
}

[Serializable]
public class Limit
{
    [SerializeField] Transform _min;
    public Transform Min => _min;

    [SerializeField] Transform _max;
    public Transform Max => _max;

    public bool Overlaps(Limit otherLimit)
    {
        return _min.position.x < otherLimit.Max.position.x && _max.position.x > otherLimit.Min.position.x &&
               _min.position.y < otherLimit.Max.position.y && _max.position.y > otherLimit.Min.position.y;
    }

    public bool Contains(Vector2 position)
    {
        return _min.position.x < position.x && _max.position.x > position.x &&
               _min.position.y < position.y && _max.position.y > position.y;
    }

    public Vector3 RandomPosition()
    {
        return new Vector3(UnityEngine.Random.Range(_min.position.x, _max.position.x), UnityEngine.Random.Range(_min.position.y, _max.position.y), 0);
    }

    public Vector2 GetCenter() => (_min.position + _max.position) / 2;
    public Vector2 GetSize() => new Vector2(Mathf.Abs(_max.position.x - _min.position.x), Mathf.Abs(_max.position.y - _min.position.y));
}
