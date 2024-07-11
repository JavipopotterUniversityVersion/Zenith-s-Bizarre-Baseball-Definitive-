using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using MyBox;
using UnityEditor.Overlays;

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
        if(roomDoorEditor.TargetMap == null) return;

        Undo.RecordObject(roomDoorEditor, "Undo Room Door Editor");

        if(GUILayout.Button("Create Door"))
        {
            Create("Door", roomDoorEditor.transform, out GameObject doorObject);
            RoomDoorData roomDoorData = doorObject.GetComponent<RoomDoorData>();
            roomDoorData.SetData(roomDoorEditor.TargetMap);
            roomDoorEditor.roomDoorData.Add(roomDoorData);
        }

        if(GUILayout.Button("Show All")) ShowAll();

        foreach(RoomDoorData roomDoor in roomDoorEditor.roomDoorData)
        {
            EditorGUILayout.BeginVertical("box");
            GUI.skin.label.fontSize = 15;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label(roomDoor.DoorIdentifier.name);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("box", GUILayout.Width(10), GUILayout.Height(10));
            if(GUILayout.Button("^", GUILayout.Width(83))) roomDoor.transform.position += Vector3.up * 2f;
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("<", GUILayout.Width(40))) roomDoor.transform.position += Vector3.left * 2f;
            if(GUILayout.Button(">", GUILayout.Width(40))) roomDoor.transform.position += Vector3.right * 2f;
            EditorGUILayout.EndHorizontal();
            if(GUILayout.Button("v", GUILayout.Width(83))) roomDoor.transform.position += Vector3.down * 2f;
            EditorGUILayout.EndVertical();


            EditorGUI.BeginChangeCheck();
            RoomAccess access = (RoomAccess)EditorGUILayout.EnumPopup("Orientation", roomDoor.DoorIdentifier.RoomAccess);
            if(EditorGUI.EndChangeCheck())
            {
                roomDoor.DoorIdentifier.SetRoomAccess(access);
                roomDoor.DoorIdentifier.name = access.ToString();
            }

            GUI.backgroundColor = Color.red;
            if(GUILayout.Button("x", GUILayout.Width(20)))
            {
                DestroyImmediate(roomDoor.DoorIdentifier.gameObject, false);
                roomDoorEditor.roomDoorData.Remove(roomDoor);
                break;
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(20);


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Success Sets");
            if(GUILayout.Button("Add Set"))
            {
                Create("Set", roomDoor.transform, out GameObject setObject);
                roomDoor.AddSuccessSet(setObject);
            }

            EditorGUILayout.EndHorizontal();
            foreach(DoorSetData doorSet in roomDoor.successSets)
            {
                EditorGUILayout.BeginHorizontal();
                doorSet.DoorSet.map = EditorGUILayout.ObjectField(doorSet.DoorSet.map, typeof(Tilemap), true) as Tilemap;
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
                roomDoor.AddFailureSet(setObject);
            }
            EditorGUILayout.EndHorizontal();
            foreach(DoorSetData doorSet in roomDoor.failureSets)
            {
                EditorGUILayout.BeginHorizontal();
                doorSet.DoorSet.map = EditorGUILayout.ObjectField(doorSet.DoorSet.map, typeof(Tilemap), true) as Tilemap;
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

    void ActivateOnlySet(DoorSetData doorSetData, RoomDoorData roomDoorData)
    {
        foreach(DoorSetData doorSet in roomDoorData.successSets.Concat(roomDoorData.failureSets))
        {
            // if(doorSetData == doorSet) SceneVisibilityManager.instance.Show(doorSet.DoorSet.gameObject, true);
            // else SceneVisibilityManager.instance.Hide(doorSet.DoorSet.gameObject, true);

            if(doorSetData == doorSet) doorSet.DoorSet.map.color = Color.white;
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
                doorSet.DoorSet.map.color = Color.white;
            }
        }
    }

    void Create(string name, Transform parent, out GameObject gameObject)
    {
        gameObject = Instantiate(Resources.Load<GameObject>("Prefabs/"+name), parent.position, Quaternion.identity, parent);
    }
}
#endif

public class RoomDoorEditor : MonoBehaviour
{
    [SerializeField] Tilemap _targetMap;
    public Tilemap TargetMap => _targetMap;

    public List<RoomDoorData> roomDoorData;
}

[Serializable]
public class DoorSetData
{
    [SerializeField] DoorSet _doorSet;
    public DoorSet DoorSet => _doorSet;

    [Range(0,1)] public float setChance;
    
    public DoorSetData(GameObject target)
    {
        _doorSet = target.GetComponent<DoorSet>();
        _doorSet.Initialize();
    }

    public static bool GetRandomDoorSet(List<DoorSetData> doorSetDatas, out DoorSet doorSet)
    {
        float totalChance = doorSetDatas.Sum(d => d.setChance);
        float randomValue = UnityEngine.Random.Range(0, totalChance);
        doorSet = null;

        float currentChance = 0;

        foreach (DoorSetData doorSetData in doorSetDatas)
        {
            currentChance += doorSetData.setChance;
            if (randomValue < currentChance)
            {
                doorSet = doorSetData.DoorSet;
                return true;
            }
        }
        return false;
    }

}