using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using MyBox;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(RoomDoorEditor))]
public class RoomDoorEditorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RoomDoorEditor roomDoorEditor = (RoomDoorEditor)target;

        if(GUILayout.Button("Create Door"))
        {
            Create("Door", roomDoorEditor.transform, out GameObject doorObject);
            RoomDoorData roomDoorData = new RoomDoorData(doorObject, roomDoorEditor.TargetMap);
            roomDoorEditor.roomDoorData.Add(roomDoorData);
        }

        if(GUILayout.Button("Show All")) ShowAll();

        foreach(RoomDoorData roomDoor in roomDoorEditor.roomDoorData)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Door");
            if(GUILayout.Button("x", GUILayout.Width(20)))
            {
                DestroyImmediate(roomDoor.DoorIdentifier.gameObject, false);
                roomDoorEditor.roomDoorData.Remove(roomDoor);
                break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            roomDoor.DoorIdentifier.SetRoomAccess((RoomAccess)EditorGUILayout.EnumPopup("Room Access", roomDoor.DoorIdentifier.RoomAccess));
            if(EditorGUI.EndChangeCheck())
            {
                roomDoor.DoorIdentifier.SetRoomAccess(roomDoor.DoorIdentifier.RoomAccess);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Success Sets");
            if(GUILayout.Button("Add Set"))
            {
                Create("Set", roomDoor.transform, out GameObject setObject);
                roomDoor.AddSuccessSet(setObject, gradient);
            }
            EditorGUILayout.EndHorizontal();
            foreach(DoorSetData doorSet in roomDoor.successSets)
            {
                EditorGUILayout.LabelField("Chance");
                EditorGUILayout.BeginHorizontal();
                // EditorGUILayout.ObjectField(doorSet.DoorSet.SetMap, typeof(Tilemap), true);
                doorSet.setChance = EditorGUILayout.Slider(doorSet.setChance, 0, 1);
                if(GUILayout.Button("Select", GUILayout.Width(70)))
                {
                    ActivateOnly(roomDoor);
                    ActivateOnlySet(doorSet, roomDoor);
                }
                if(GUILayout.Button("Remove"))
                {
                    DestroyImmediate(doorSet.DoorSet.gameObject, false);
                    roomDoor.RemoveSuccessSet(doorSet);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(30);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Failure Sets");
            if(GUILayout.Button("Add Set"))
            {
                Create("Set", roomDoor.transform, out GameObject setObject);
                roomDoor.AddFailureSet(setObject, gradient);
            }
            EditorGUILayout.EndHorizontal();
            foreach(DoorSetData doorSet in roomDoor.failureSets)
            {
                EditorGUILayout.LabelField("Chance");
                EditorGUILayout.BeginHorizontal();
                // EditorGUILayout.ObjectField(doorSet.DoorSet.SetMap, typeof(Tilemap), true);
                doorSet.setChance = EditorGUILayout.Slider(doorSet.setChance, 0, 1);
                if(GUILayout.Button("Select", GUILayout.Width(70)))
                {
                    ActivateOnly(roomDoor);
                    ActivateOnlySet(doorSet, roomDoor);
                }
                if(GUILayout.Button("Remove"))
                {
                    DestroyImmediate(doorSet.DoorSet.gameObject, false);
                    roomDoor.RemoveFailureSet(doorSet);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }
    }

    void ActivateOnly(RoomDoorData roomDoorData)
    {
        RoomDoorEditor roomDoorEditor = (RoomDoorEditor)target;
        foreach(RoomDoorData roomDoor in roomDoorEditor.roomDoorData)
        {
            if(roomDoorData == roomDoor) SceneVisibilityManager.instance.Show(roomDoor.DoorIdentifier.gameObject, true);
            else SceneVisibilityManager.instance.Hide(roomDoor.DoorIdentifier.gameObject, true);
        }
    }

    Gradient gradient => ((RoomDoorEditor)target).gradient;

    void ActivateOnlySet(DoorSetData doorSetData, RoomDoorData roomDoorData)
    {
        foreach(DoorSetData doorSet in roomDoorData.successSets.Concat(roomDoorData.failureSets))
        {
            // if(doorSetData == doorSet) SceneVisibilityManager.instance.Show(doorSet.DoorSet.gameObject, true);
            // else SceneVisibilityManager.instance.Hide(doorSet.DoorSet.gameObject, true);

            if(doorSetData == doorSet) doorSet.DoorSet.SetMap.color = Color.white;
            else doorSet.DoorSet.ColorItSelf();
        }

        Selection.activeGameObject = doorSetData.DoorSet.gameObject;
    }

    void ShowAll()
    {
        SceneVisibilityManager.instance.ShowAll();
        ResetAllColors();
    }

    void ResetAllColors()
    {
        foreach(RoomDoorData roomDoor in ((RoomDoorEditor)target).roomDoorData)
        {
            foreach(DoorSetData doorSet in roomDoor.successSets.Concat(roomDoor.failureSets))
            {
                doorSet.DoorSet.SetMap.color = Color.white;
            }
        }
    }

    void Create(string name, Transform parent, out GameObject gameObject)
    {
        gameObject = new GameObject(name);
        gameObject.transform.SetParent(parent);
        gameObject.transform.localPosition = Vector3.zero;
    }
}
#endif

public class RoomDoorEditor : MonoBehaviour
{
    public Gradient gradient;
    [SerializeField] Tilemap _targetMap;
    public Tilemap TargetMap => _targetMap;

    [HideInInspector] public List<RoomDoorData> roomDoorData;
}

[Serializable]
public class RoomDoorData
{
    [HideInInspector] public List<DoorSetData> successSets;
    [HideInInspector] public List<DoorSetData> failureSets;

    DoorIdentifier _doorIdentifier;
    public DoorIdentifier DoorIdentifier => _doorIdentifier;

    public Transform transform => _doorIdentifier.transform;

    public void SuccessSet(Tilemap targetMap)
    {
        DoorSet doorSet = DoorSetData.GetRandomDoorSet(successSets);
        doorSet.Set(targetMap);
    }

    public void FailureSet(Tilemap targetMap)
    {
        DoorSet doorSet = DoorSetData.GetRandomDoorSet(failureSets);
        doorSet.Set(targetMap);
    }

    public RoomDoorData (GameObject targetObject, Tilemap targetMap, RoomAccess access = RoomAccess.North)
    {
        _doorIdentifier = targetObject.AddComponent<DoorIdentifier>();
        _doorIdentifier.SetRoomAccess(access);

        successSets = new List<DoorSetData>();
        failureSets = new List<DoorSetData>();

        _doorIdentifier.OnVerifyIdentity.AddListener(() => SuccessSet(targetMap));
        _doorIdentifier.OnFailIdentity.AddListener(() => FailureSet(targetMap));
    }

    public void AddSuccessSet(GameObject targetObject, Gradient gradient)
    {
        successSets.Add(new DoorSetData(targetObject,gradient));
    }
    public void AddFailureSet(GameObject targetObject, Gradient gradient)
    {
        failureSets.Add(new DoorSetData(targetObject,gradient));
    }

    public void RemoveSuccessSet(DoorSetData doorSet) => successSets.Remove(doorSet);
    public void RemoveFailureSet(DoorSetData doorSet) => failureSets.Remove(doorSet);
}

[Serializable]
public class DoorSetData
{
    [SerializeField] DoorSet _doorSet;
    public DoorSet DoorSet => _doorSet;

    [Range(0,1)] public float setChance;
    
    public DoorSetData(GameObject target, Gradient gradient)
    {
        _doorSet = new DoorSet(target);
        _doorSet.ownColor = gradient.Evaluate(UnityEngine.Random.Range(0, 1));
    }

    public static DoorSet GetRandomDoorSet(List<DoorSetData> doorSets)
    {
        float totalChance = doorSets.Sum(d => d.setChance);
        float randomValue = UnityEngine.Random.Range(0, totalChance);

        float currentChance = 0;

        foreach (DoorSetData doorSet in doorSets)
        {
            currentChance += doorSet.setChance;
            if (randomValue < currentChance)
            {
                return doorSet._doorSet;
            }
        }
        return null;
    }

}

[Serializable]
public class DoorSet
{
    Tilemap _setMap;
    public Tilemap SetMap => _setMap;
    public GameObject gameObject => _setMap.gameObject;
    [HideInInspector] public Color ownColor;

    public void ColorItSelf() => _setMap.color = ownColor;

    public DoorSet(GameObject target)
    {
        _setMap = target.AddComponent<Tilemap>();
        target.AddComponent<TilemapRenderer>();
        target.AddComponent<Grid>().cellSize = new Vector3(2, 2, 0);
    }

    public Vector3Int[] GetPositions()
    {
        List<Vector3Int> setPositions = new List<Vector3Int>();
        foreach (Vector3Int position in _setMap.cellBounds.allPositionsWithin)
        {
            if (_setMap.HasTile(position))
            {
                setPositions.Add(position);
            }
        }
        return setPositions.ToArray();
    }

    public void Set(Tilemap targetMap)
    {
        foreach (Vector3Int position in GetPositions())
        {
            targetMap.SetTile(position, _setMap.GetTile(position));
        }
    }
}