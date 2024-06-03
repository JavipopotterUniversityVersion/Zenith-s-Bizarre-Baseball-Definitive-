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
    [SerializeField] ContactFilter2D _contactFilter;
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

    int extensionIndex = 0;
    public int ExtensionIndex => extensionIndex;

    string[] names = new string[] {"Pepe", "Juan", "Pedro", "Luis", "Carlos", "Jorge", "Ricardo", "Miguel", "Alberto", "Fernando", "Rob", "John", "Mike", "Steve", "Tom", "Jerry", "Rick", "Morty", "Beth", "Summer", "Jerry", "Birdperson", "Tammy", "Squanchy", "Unity", "Mr. Poopybutthole", "Noob Noob", "Scary Terry", "Abradolf Lincler", "Pencilvester", "Photography Raptor", "Crocubot", "Gearhead", "Million Ants", "Trunk People", "Gazorpazorpfield", "Ants in my Eyes Johnson", "Reverse Giraffe", "Hamurai", "Amish Cyborg", "Purge Planet Ruler", "Cromulon", "Gromflomite", "Plutonian", "Zigerion", "Meeseeks", "Cronenberg", "Fart", "Giant Head", "Giant Testicle Monster", "Giant Arm", "Giant Cat", "Giant Beetle", "Giant Spider"};

    private void Awake() {
        name = names[UnityEngine.Random.Range(0, names.Length)];
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

        for (int i = 0; i < _gates.Length; i++)
        {
            if(extension <= 0 || branchExtension <= 0)
            {
                CloseNodes();
                return;
            }
            
            Gate gate = _gates[i];

            if (gate.IsConnected || !Access.HasFlag(gate.Access)) continue;

            GameObject nodePrefab = NodeSetting.RandomNodeSetting(nodeSettings);
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
                Destroy(node.gameObject);
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

    public bool IsConnected => _connectedGate != null;

    [SerializeField] Gate _connectedGate;
    public Gate ConnectedGate
    {
        get => _connectedGate;
        set => _connectedGate = value;
    }

    [SerializeField] RoomAccess _access;
    public RoomAccess Access => _access;
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

    [SerializeField] int minExtension;
    public int MinExtension => minExtension;

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

    public bool Contains(Vector3 position)
    {
        return _min.position.x < position.x && _max.position.x > position.x &&
               _min.position.y < position.y && _max.position.y > position.y;
    }

    public Vector3 RandomPosition()
    {
        return new Vector3(UnityEngine.Random.Range(_min.position.x, _max.position.x), UnityEngine.Random.Range(_min.position.y, _max.position.y), 0);
    }
}
