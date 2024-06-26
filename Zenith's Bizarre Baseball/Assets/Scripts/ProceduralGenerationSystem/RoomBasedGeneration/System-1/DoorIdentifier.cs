using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DoorIdentifier : MonoBehaviour
{
    [SerializeField] private RoomAccess roomAccess;
    public RoomAccess RoomAccess => roomAccess;

    private Node _DoorRoom;
    [SerializeField] UnityEvent onVerifyIdentity = new UnityEvent();
    [SerializeField] UnityEvent onFailIdentity = new UnityEvent();

    private void Awake() => StartCoroutine(Check());

    private IEnumerator Check() {
        yield return null;
        SetPositionAccess();
        _DoorRoom = GetComponentInParent<Node>();
        _DoorRoom.Generator.OnFinishedGeneration.AddListener(CompareFlag);
    }
    private void CompareFlag() 
    {
        if(roomAccess == 0) roomAccess = PositionRoomAccess();
        if (_DoorRoom.Access.HasFlag(roomAccess))
        {
            onVerifyIdentity?.Invoke();
        }
        else
        {
            onFailIdentity?.Invoke();
        }
    }

    public RoomAccess SetPositionAccess()
    {
        roomAccess = PositionRoomAccess();
        return roomAccess;
    }

    private RoomAccess PositionRoomAccess()
    {
        if(transform.up.y > transform.up.x)
        {
            if(transform.up.y > 0) return RoomAccess.East;
            else return RoomAccess.North;
        }
        else
        {
            if(transform.up.x > 0) return RoomAccess.South;
            else return RoomAccess.West;
        }
    }

    private void OnValidate() {
        name = name = $"Door {SetPositionAccess()}";
    }

    // private void OnDrawGizmos() => OnValidate();
}
