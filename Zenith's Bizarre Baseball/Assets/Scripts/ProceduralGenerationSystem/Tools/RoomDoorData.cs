using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomDoorData : MonoBehaviour
{
    [HideInInspector] public List<DoorSetData> successSets = new List<DoorSetData>();
    [HideInInspector] public List<DoorSetData> failureSets = new List<DoorSetData>();
    [SerializeField] Tilemap _targetMap;

    [SerializeField] DoorIdentifier _doorIdentifier;
    public DoorIdentifier DoorIdentifier => _doorIdentifier;

    public void SuccessSet()
    {
        if(DoorSetData.GetRandomDoorSet(successSets, out DoorSet doorSet))
        {
            doorSet.Set(_targetMap);
        }
    }

    public void FailureSet()
    {
        if(DoorSetData.GetRandomDoorSet(failureSets, out DoorSet doorSet))
        {
            doorSet.Set(_targetMap);
        }
    }

    public void SetData (Tilemap targetMap, RoomAccess access = RoomAccess.None)
    {
        _doorIdentifier.SetRoomAccess(access);
        _targetMap = targetMap;
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