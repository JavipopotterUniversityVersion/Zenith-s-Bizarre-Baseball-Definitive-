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
    [SerializeField] DoorIdentifier[] _doors;
    public DoorIdentifier[] Doors => _doors;

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

    [SerializeField] List<Limit> _limits = new List<Limit>();
    public List<Limit> LimitsList => _limits;

    public Limit[] Limits => _limits.ToArray();

    string[] names = new string[] {"Pepe", "Juan", "Pedro", "Luis", "Carlos", "Jorge", "Ricardo", "Miguel", "Alberto", "Fernando", "Rob", "John", "Mike", "Steve", "Tom", "Jerry", "Rick", "Morty", "Beth", "Summer", "Jerry", "Birdperson", "Tammy", "Squanchy", "Unity", "Mr. Poopybutthole", "Noob Noob", "Scary Terry", "Abradolf Lincler", "Pencilvester", "Photography Raptor", "Crocubot", "Gearhead", "Million Ants", "Trunk People", "Gazorpazorpfield", "Ants in my Eyes Johnson", "Reverse Giraffe", "Hamurai", "Amish Cyborg", "Purge Planet Ruler", "Cromulon", "Gromflomite", "Plutonian", "Zigerion", "Meeseeks", "Cronenberg", "Fart", "Giant Head", "Giant Testicle Monster", "Giant Arm", "Giant Cat", "Giant Beetle", "Giant Spider"};

    private void Awake() {
        name = names[UnityEngine.Random.Range(0, names.Length)];
    }

    void OnDrawGizmos()
    {
        foreach(Limit limit in _limits)
        {
            Gizmos.color = limit.color;

            if(limit.Min.position.x > limit.Max.position.x) limit.Min.position = new Vector2(limit.Max.position.x, limit.Min.position.y);
            else if(limit.Min.position.y > limit.Max.position.y) limit.Min.position = new Vector2(limit.Min.position.x, limit.Max.position.y);

            Gizmos.DrawWireCube(limit.GetCenter(), limit.GetSize());
        }
    }

    private void OnValidate() {
        _doors = GetComponentsInChildren<DoorIdentifier>();
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
        await Task.Delay(80);
    }

    public async void GenerateNodes(float linearity, NodeGenerator generator, int branchExtension)
    {
        _generator = generator;
        _linearity = linearity;

        nodeSettings.AddRange(generator.NodeSettings.ToList());
        extensionIndex = _generator.Extension - branchExtension;
        if(extensionIndex > largestBranch) largestBranch = extensionIndex;

        List<DoorIdentifier> posibleDoors = _doors.Where(door => !door.conected && Access.HasFlag(door.RoomAccess)).ToList();
        

        for (int i = 0; i < posibleDoors.Count; i++)
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
            
            DoorIdentifier gate = posibleDoors[i];

            List<NodeSetting> possibleNodes = nodeSettings;
            // possibleNodes.AddRange(gate.possibleNodes);

            NodeSetting setting = NodeSetting.RandomNodeSetting(possibleNodes);
            if (setting == null)
            {
                CloseNodes();
                return;
            }

            GameObject nodePrefab = setting.NodePrefab;
            Node node = Instantiate(nodePrefab).GetComponent<Node>();
            node.Generator = _generator;

            int j = 0;

            while (!node.ConnectNodes(ref gate))
            {
                if(Application.isEditor) DestroyImmediate(node.gameObject, false);
                else
                {
                    DumpNode(node);
                }

                setting = NodeSetting.RandomNodeSetting(possibleNodes);
                node = Instantiate(setting.NodePrefab.GetComponent<Node>());
                node.Generator = _generator;

                j++;

                if (j > 10)
                {
                    DumpNode(node);
                    CloseAccess(gate.RoomAccess);
                    break;
                }
            }

            if (j > 10) continue;

            _generator.Nodes.Add(node);
            setting.TimesAppeared++;

            node.SetAccess(ReturnRandomAccess(GetOppositeAccess(gate.RoomAccess)));
            extension--;

            await Task.Delay(1/(branchExtension+1));
            node.GenerateNodes(_linearity, _generator, branchExtension - 1);
        }
    }

    void DumpNode(Node node)
    {
        node.gameObject.SetActive(false);
        Generator.unusedNodes.Add(node.gameObject);
    }

    void CloseNodes()
    {
        foreach(DoorIdentifier gate in _doors)
        {
            if(!gate.conected) CloseAccess(gate.RoomAccess);
        }
    }

    public bool TryPlaceNode(GameObject nodePrefab)
    {
        DoorIdentifier[] _doors = GetAvailableGates();
        if(_doors.Length == 0) return false;

        Node node = Instantiate(nodePrefab).GetComponent<Node>();
        node.Generator = _generator;

        int i = 0;
        bool result = false;

        DoorIdentifier gate = _doors[i];

        while(i < _doors.Length && !result)
        {
            gate = _doors[i];
            OpenAccess(gate.RoomAccess);
            result = node.ConnectNodes(ref gate);
            i++;
        }


        if(!result)
        {
            DumpNode(node);
            CloseAccess(gate.RoomAccess);
        }
        else
        {
            node.SetAccess(GetOppositeAccess(gate.RoomAccess));
            _generator.Nodes.Add(node);
        }

        return result;
    }

    DoorIdentifier[] GetAvailableGates()
    {
        return _doors.Where(door => !door.conected).ToArray();
    }
   
    void CloseAccess(RoomAccess access) => Access &= ~access;
    void OpenAccess(RoomAccess access) => Access |= access;

    bool CanPlaceNode()
    {
        return !_generator.CheckIntersecctions(this);
    }

    public bool ConnectNodes(ref DoorIdentifier gate)
    {
        bool found = false;
        foreach(DoorIdentifier g in _doors)
        {
            if(GetOppositeAccess(g.RoomAccess) == gate.RoomAccess)
            {
                g.connectedDoor = gate;
                gate.connectedDoor = g;
                Vector3 nodePosition = gate.transform.position + (transform.position - g.transform.position);
                transform.position = nodePosition;
                found = true;
                break;
            }
        }

        if(!found) return false;

        if(!CanPlaceNode())
        {
            gate.connectedDoor = null;
            return false;
        }
        else return true;
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

    public static NodeSetting RandomNodeSetting(List<NodeSetting> nodeSettings) => RandomNodeSetting(nodeSettings.ToArray());

    public static NodeSetting RandomNodeSetting(NodeSetting[] nodeSettings)
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
                return setting;
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

    public Color color = Color.magenta;

    public Limit(Transform min, Transform max, Color color)
    {
        _min = min;
        _max = max;
        this.color = color;
    }

    public void SetLimits(Transform min, Transform max)
    {
        _min = min;
        _max = max;
    }

    public void CopyLimits(Limit otherLimit)
    {
        _min.localPosition = otherLimit.Min.localPosition;
        _max.localPosition = otherLimit.Max.localPosition;
    }

    public bool Overlaps(Limit otherLimit)
    {
        return _min.position.x < otherLimit.Max.position.x && _max.position.x > otherLimit.Min.position.x &&
               _min.position.y < otherLimit.Max.position.y && _max.position.y > otherLimit.Min.position.y;
    }

    public bool OverlapsInclusive(Limit otherLimit)
    {
        return _min.position.x <= otherLimit.Max.position.x && _max.position.x >= otherLimit.Min.position.x &&
               _min.position.y <= otherLimit.Max.position.y && _max.position.y >= otherLimit.Min.position.y;
    }

    public bool Contains(Vector2 position, float margin = 0)
    {
        return _min.position.x - margin < position.x && _max.position.x + margin > position.x &&
               _min.position.y - margin < position.y && _max.position.y + margin > position.y;
    }

    public Vector3 RandomPosition()
    {
        return new Vector3(UnityEngine.Random.Range(_min.position.x, _max.position.x), UnityEngine.Random.Range(_min.position.y, _max.position.y), 0);
    }

    public Vector2 GetCenter() => (_min.position + _max.position) / 2;
    public Vector2 GetSize() => new Vector2(Mathf.Abs(_max.position.x - _min.position.x), Mathf.Abs(_max.position.y - _min.position.y));

    public static bool Contains(Vector2 position, Limit[] limits, float margin = 0)
    {
        foreach(Limit limit in limits)
        {
            if(limit.Contains(position, margin)) return true;
        }
        return false;
    }

    public static bool Overlaps(Limit limit, Limit[] limits)
    {
        bool value = false;
        int i = 0;

        while(i < limits.Length && value == false)
        {
            value = limit.Overlaps(limits[i]);
            i++;
        }

        return value;
    }

    public static bool OverlapsInclusive(Limit limit, Limit[] limits)
    {
        bool value = false;
        int i = 0;

        while(i < limits.Length && value == false)
        {
            value = limit.OverlapsInclusive(limits[i]);
            i++;
        }

        return value;
    }
}
