using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using MyBox;
using UnityEngine.UIElements;
using UnityEngine.Events;



#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(RoomDoorEditor))]
public class RoomDoorEditorEditor : Editor
{
  RoomDoorData selectedDoor;
  Limit selectedLimit;
  string limitName = "Limit";

  private void row(UnityAction callback)
  {
    EditorGUILayout.BeginHorizontal();
    callback();
    EditorGUILayout.EndHorizontal();
  }

  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    RoomDoorEditor roomDoorEditor = (RoomDoorEditor)target;
    if (roomDoorEditor.TargetMap == null) return;

    Undo.RecordObject(roomDoorEditor, "Undo Room Door Editor");

    this.row(() =>
    {
      foreach (RoomAccess access in Enum.GetValues(typeof(RoomAccess)))
      {
        if (access != RoomAccess.None && GUILayout.Button("Add " + access.ToString() + " Door"))
        {
          Create("Door", roomDoorEditor.transform, out GameObject doorObject);
          RoomDoorData roomDoorData = doorObject.GetComponent<RoomDoorData>();
          roomDoorData.SetData(roomDoorEditor.TargetMap, access);
          roomDoorData.name = access.ToString();
          roomDoorEditor.roomDoorData.Add(roomDoorData);
        }
      }
    });

    if (GUILayout.Button("Select Main Map")) Select(roomDoorEditor.TargetMap.gameObject);
    if (GUILayout.Button("Select Hole Map")) Select(roomDoorEditor.HoleMap.gameObject);

    if (GUILayout.Button("Show All")) ShowAll();

    if (selectedDoor == null)
    {
      if (roomDoorEditor.roomDoorData.Count > 0) selectedDoor = roomDoorEditor.roomDoorData[roomDoorEditor.roomDoorData.Count - 1];
      if (selectedDoor == null) return;
    }

    EditorGUILayout.BeginHorizontal();

    {
      EditorGUILayout.BeginVertical("box");
      GUI.skin.label.fontSize = 15;
      GUI.skin.label.alignment = TextAnchor.MiddleCenter;
      GUILayout.Label(selectedDoor.DoorIdentifier.name);

      EditorGUILayout.BeginHorizontal();

      EditorGUI.BeginChangeCheck();
      RoomAccess access = (RoomAccess)EditorGUILayout.EnumPopup("Orientation", selectedDoor.DoorIdentifier.RoomAccess);
      if (EditorGUI.EndChangeCheck())
      {
        selectedDoor.DoorIdentifier.SetRoomAccess(access);
        selectedDoor.DoorIdentifier.name = access.ToString();
      }
      EditorGUILayout.EndHorizontal();

      {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Success Sets");
        if (GUILayout.Button("Add Set"))
        {
          Create("Set", selectedDoor.transform, out GameObject setObject);
          selectedDoor.AddSuccessSet(setObject);
        }
        EditorGUILayout.EndHorizontal();

        foreach (DoorSetData doorSet in selectedDoor.successSets)
        {
          EditorGUILayout.BeginHorizontal();
          doorSet.DoorSet.map = EditorGUILayout.ObjectField(doorSet.DoorSet.map, typeof(Tilemap), true) as Tilemap;
          doorSet.setChance = EditorGUILayout.Slider(doorSet.setChance, 0, 1);
          if (GUILayout.Button("Select", GUILayout.Width(70)))
          {
            ActivateOnly(selectedDoor);
            ActivateOnlySet(doorSet, selectedDoor);
          }
          if (GUILayout.Button("Remove"))
          {
            DestroyImmediate(doorSet.DoorSet.gameObject, false);
            selectedDoor.RemoveSuccessSet(doorSet);
            break;
          }
          EditorGUILayout.EndHorizontal();
        }
      }

      EditorGUILayout.Space(40);

      {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Failure Sets");
        if (GUILayout.Button("Add Set"))
        {
          Create("Set", selectedDoor.transform, out GameObject setObject);
          selectedDoor.AddFailureSet(setObject);
        }
        EditorGUILayout.EndHorizontal();
        foreach (DoorSetData doorSet in selectedDoor.failureSets)
        {
          EditorGUILayout.BeginHorizontal();
          doorSet.DoorSet.map = EditorGUILayout.ObjectField(doorSet.DoorSet.map, typeof(Tilemap), true) as Tilemap;
          doorSet.setChance = EditorGUILayout.Slider(doorSet.setChance, 0, 1);
          if (GUILayout.Button("Select", GUILayout.Width(70)))
          {
            ActivateOnly(selectedDoor);
            ActivateOnlySet(doorSet, selectedDoor);
          }
          if (GUILayout.Button("Remove"))
          {
            DestroyImmediate(doorSet.DoorSet.gameObject, false);
            selectedDoor.RemoveFailureSet(doorSet);
            break;
          }
          EditorGUILayout.EndHorizontal();
        }
      }

      EditorGUILayout.BeginVertical("box");

      EditorGUI.BeginChangeCheck();

      RandomFixedPositionOnAwake randomPosOnAwake = selectedDoor.GetComponent<RandomFixedPositionOnAwake>();
      if (GUILayout.Button("Add new position"))
      {
        randomPosOnAwake.posiblePositions.Add(Vector2Int.zero);
      }

      Undo.RecordObject(selectedDoor.DoorIdentifier, "Undo Door Variability");

      for (int i = 0; i < randomPosOnAwake.posiblePositions.Count; i++)
      {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("^", GUILayout.Width(40))) randomPosOnAwake.posiblePositions[i] += Vector2Int.up;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("<", GUILayout.Width(20))) randomPosOnAwake.posiblePositions[i] += Vector2Int.left;
        if (GUILayout.Button(">", GUILayout.Width(20))) randomPosOnAwake.posiblePositions[i] += Vector2Int.right;
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("v", GUILayout.Width(40))) randomPosOnAwake.posiblePositions[i] += Vector2Int.down;
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("x", GUILayout.Width(20)))
        {
          randomPosOnAwake.posiblePositions.RemoveAt(i);
          break;
        }
        EditorGUILayout.EndHorizontal();
      }

      if (EditorGUI.EndChangeCheck())
      {
        EditorUtility.SetDirty(selectedDoor.DoorIdentifier);
      }

      EditorGUILayout.EndVertical();

      EditorGUILayout.EndVertical();
    }

    EditorGUILayout.BeginVertical("box", GUILayout.Width(100));
    //Door Movement
    {
      EditorGUILayout.BeginVertical("box", GUILayout.Width(10), GUILayout.Height(10));
      if (GUILayout.Button("^", GUILayout.Width(83))) selectedDoor.transform.position += Vector3.up * 2f;
      EditorGUILayout.BeginHorizontal();
      if (GUILayout.Button("<", GUILayout.Width(40))) selectedDoor.transform.position += Vector3.left * 2f;
      if (GUILayout.Button(">", GUILayout.Width(40))) selectedDoor.transform.position += Vector3.right * 2f;
      EditorGUILayout.EndHorizontal();
      if (GUILayout.Button("v", GUILayout.Width(83))) selectedDoor.transform.position += Vector3.down * 2f;
      EditorGUILayout.EndVertical();
    }

    //Door Tabs
    foreach (RoomDoorData d in roomDoorEditor.roomDoorData)
    {
      EditorGUILayout.BeginHorizontal();
      if (selectedDoor == d) GUI.backgroundColor = Color.gray;
      else GUI.backgroundColor = Color.white;
      if (GUILayout.Button(d.name, GUILayout.Width(100), GUILayout.MinWidth(10))) selectedDoor = d;

      GUI.backgroundColor = Color.red;
      if (GUILayout.Button("x", GUILayout.Width(20)))
      {
        DestroyImmediate(selectedDoor.DoorIdentifier.gameObject, false);
        roomDoorEditor.roomDoorData.Remove(selectedDoor);
        break;
      }
      GUI.backgroundColor = Color.white;
      EditorGUILayout.EndHorizontal();
    }


    EditorGUILayout.EndVertical();
    EditorGUILayout.EndHorizontal();

    EditorGUILayout.BeginVertical("box");
    RoomNode node = roomDoorEditor.GetComponentInParent<RoomNode>();

    EditorGUI.BeginChangeCheck();

    if (GUILayout.Button("Create Limit"))
    {
      if (roomDoorEditor.limitsContainer == null)
      {
        roomDoorEditor.limitsContainer = new GameObject();
        roomDoorEditor.limitsContainer.transform.parent = node.transform;
        roomDoorEditor.limitsContainer.transform.position = node.transform.position;
        roomDoorEditor.limitsContainer.name = "Limits";
      }

      Create("Limit", roomDoorEditor.limitsContainer.transform, out GameObject limitObject);
      Create("min", limitObject.transform, out GameObject minObject);
      Create("max", limitObject.transform, out GameObject maxObject);

      Gradient gradient = new Gradient();

      gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.magenta, 1.0f), new GradientColorKey(Color.cyan, 0f) },
      new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });

      gradient.mode = GradientMode.Blend;
      Color color = gradient.Evaluate(UnityEngine.Random.value);
      node.LimitsList.Add(new Limit(minObject.transform, maxObject.transform, color));
    }


    {
      if (selectedLimit == null)
      {
        if (node.LimitsList.Count > 0) selectedLimit = node.LimitsList[node.LimitsList.Count - 1];
        if (selectedLimit == null)
        {
          EditorGUILayout.EndVertical();
          return;
        }
      }

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.BeginVertical("box");
      GUI.skin.label.alignment = TextAnchor.MiddleCenter;
      GUILayout.Label(limitName);
      GUI.skin.label.alignment = TextAnchor.MiddleLeft;
      EditorGUILayout.BeginHorizontal();

      if (GUILayout.Button("Select Min")) Select(selectedLimit.Min.gameObject);
      if (GUILayout.Button("Select Max")) Select(selectedLimit.Max.gameObject);

      EditorGUILayout.EndHorizontal();
      EditorGUILayout.EndVertical();
    }

    {
      EditorGUILayout.BeginVertical("box", GUILayout.Width(100));
      for (int i = 0; i < node.LimitsList.Count; i++)
      {
        EditorGUILayout.BeginHorizontal();
        if (selectedLimit == node.LimitsList[i])
        {
          GUI.backgroundColor = Color.gray;
          limitName = "Limit " + i;
        }
        else GUI.backgroundColor = Color.white;
        if (GUILayout.Button("Limit " + i, GUILayout.Width(100), GUILayout.MinWidth(10))) selectedLimit = node.LimitsList[i];
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("x", GUILayout.Width(20)))
        {
          GameObject objectToDestroy = node.LimitsList[i].Min.transform.parent.gameObject;
          DestroyImmediate(objectToDestroy, false);
          node.LimitsList.Remove(node.LimitsList[i]);
          break;
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndVertical();
    }

    if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(node);

    EditorGUILayout.EndHorizontal();
    EditorGUILayout.EndVertical();

    GUI.skin.label.fontSize = 10;
  }

  void ActivateOnly(RoomDoorData roomDoorData)
  {
    RoomDoorEditor roomDoorEditor = (RoomDoorEditor)target;
    foreach (RoomDoorData roomDoor in roomDoorEditor.roomDoorData)
    {
      if (roomDoorData == roomDoor) SceneVisibilityManager.instance.Show(roomDoor.DoorIdentifier.gameObject, true);
      else SceneVisibilityManager.instance.Hide(roomDoor.DoorIdentifier.gameObject, true);
    }
  }

  void ActivateOnlySet(DoorSetData doorSetData, RoomDoorData roomDoorData)
  {
    foreach (DoorSetData doorSet in roomDoorData.successSets.Concat(roomDoorData.failureSets))
    {
      if (doorSetData == doorSet) doorSet.DoorSet.ResetColor();
      else doorSet.DoorSet.ColorItSelf();
    }

    Select(doorSetData.DoorSet.gameObject);
  }

  void Select(GameObject objectToSelect)
  {
    ActiveEditorTracker.sharedTracker.isLocked = true;
    Selection.activeGameObject = objectToSelect;
  }

  void ShowAll()
  {
    SceneVisibilityManager.instance.ShowAll();
    ResetAllColors();
  }

  void ResetAllColors()
  {
    foreach (RoomDoorData roomDoor in ((RoomDoorEditor)target).roomDoorData)
    {
      foreach (DoorSetData doorSet in roomDoor.successSets.Concat(roomDoor.failureSets))
      {
        doorSet.DoorSet.ResetColor();
      }
    }
  }

  void Create(string name, Transform parent, out GameObject gameObject)
  {
    gameObject = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Prefabs/" + name), parent.transform) as GameObject;
    gameObject.transform.position = parent.position;
  }

  void OnDisable() => ShowAll();
}
#endif

public class RoomDoorEditor : MonoBehaviour
{
  [SerializeField] Tilemap _targetMap;
  public Tilemap TargetMap => _targetMap;

  [SerializeField] Tilemap _holeMap;
  public Tilemap HoleMap => _holeMap;

  public GameObject limitsContainer;
  public List<RoomDoorData> roomDoorData;
}

[Serializable]
public class DoorSetData
{
  [SerializeField] DoorSet _doorSet;
  public DoorSet DoorSet => _doorSet;

  [Range(0, 1)] public float setChance;

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