using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;

public class Node : MonoBehaviour
{
    NodeGenerator _generator;
    public NodeGenerator Generator {set => _generator = value; get => _generator;}
    [SerializeField] Gate[] _gates;
    public Gate[] Gates => _gates;
    [SerializeField] ContactFilter2D _contactFilter;
    [SerializeField] NodeSetting[] nodeSettings;
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

    string[] names = new string[] {"Pepe", "Juan", "Pedro", "Luis", "Carlos", "Jorge", "Ricardo", "Miguel", "Alberto", "Fernando", "Rob", "John", "Mike", "Steve", "Tom", "Jerry", "Rick", "Morty", "Beth", "Summer", "Jerry", "Birdperson", "Tammy", "Squanchy", "Unity", "Mr. Poopybutthole", "Noob Noob", "Scary Terry", "Abradolf Lincler", "Pencilvester", "Photography Raptor", "Crocubot", "Gearhead", "Million Ants", "Trunk People", "Gazorpazorpfield", "Ants in my Eyes Johnson", "Reverse Giraffe", "Hamurai", "Amish Cyborg", "Purge Planet Ruler", "Cromulon", "Gromflomite", "Plutonian", "Zigerion", "Meeseeks", "Cronenberg", "Fart", "Giant Head", "Giant Testicle Monster", "Giant Arm", "Giant Cat", "Giant Beetle", "Giant Spider"};

    private void Awake() {
        name = names[UnityEngine.Random.Range(0, names.Length)];
    }

    public void SetAccess(RoomAccess access) => Access = access;
    RoomAccess ReturnRandomAccess(RoomAccess accessValue)
    {
        if(UnityEngine.Random.value < _linearity) return accessValue |= GetOppositeAccess(accessValue);

        RoomAccess newAccess = (RoomAccess) UnityEngine.Random.Range(1, 15);
        while (accessValue == newAccess)
        {           
            newAccess = (RoomAccess) UnityEngine.Random.Range(1, 15);
        }
        accessValue |= newAccess;
        return accessValue;
    }

    public async Task GenerateNodes(float linearity, NodeGenerator generator, int branchExtension)
    {
        _generator = generator;
        _linearity = linearity;


        for (int i = 0; i < _gates.Length; i++)
        {
            if(extension <= 0 || branchExtension <= 0)
            {
                CloseNodes();
                return;
            }
            
            Gate gate = _gates[i];

            if (gate.IsConnected || !Access.HasFlag(gate.Access)) continue;

            Node node = Instantiate(NodeSetting.RandomNodeSetting(nodeSettings)).GetComponent<Node>();
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
            await Task.Delay(10);
            await node.GenerateNodes(_linearity, _generator, branchExtension - 1);
        }
    }

    void CloseNodes()
    {
        foreach(Gate gate in _gates)
        {
            if(!gate.IsConnected) CloseAccess(gate.Access);
        }
    }

    void CloseAccess(RoomAccess access) => Access &= ~access;

    bool CanPlaceNode()
    {
        return !_generator.CheckIntersecctions(this);
    }

    public Limit[] limits;

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

    [SerializeField] [Range(0, 1)]
    float _probability;

    public float Probability => _probability;

    [SerializeField] int _minNumberOfNodes;
    public int MinNumberOfNodes => _minNumberOfNodes;

    [SerializeField] int _maxNumberOfNodes;
    public int MaxNumberOfNodes => _maxNumberOfNodes;

    public static GameObject RandomNodeSetting(NodeSetting[] nodeSettings)
    {
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
            if(randomValue <= currentProbability) return setting._nodePrefab;
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

    public bool IsInside(Limit otherLimit)
    {
        return _min.position.x < otherLimit.Max.position.x && _max.position.x > otherLimit.Min.position.x &&
               _min.position.y < otherLimit.Max.position.y && _max.position.y > otherLimit.Min.position.y;
    }
}
