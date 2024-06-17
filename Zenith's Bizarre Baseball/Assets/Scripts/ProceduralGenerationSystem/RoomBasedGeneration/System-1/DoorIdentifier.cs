using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DoorIdentifier : MonoBehaviour
{
    [SerializeField] private RoomAccess roomAccess;
    private Node _DoorRoom;
    [SerializeField] UnityEvent onVerifyIdentity = new UnityEvent();
    [SerializeField] UnityEvent onFailIdentity = new UnityEvent();

    private void Awake() => StartCoroutine(Check());

    private IEnumerator Check() {
        yield return null;
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
    private RoomAccess PositionRoomAccess()
    {
        if (Math.Abs(transform.localPosition.x) > Math.Abs(transform.localPosition.y))
        {
            if (transform.localPosition.x > 0) 
            {               
                return RoomAccess.East;
            }
            else 
            {
                return RoomAccess.West;
            }
        }
        else
        {
            if (transform.localPosition.y > 0)
            {
                return RoomAccess.North;
            }
            else
            {
                return RoomAccess.South;
            }
        }
    }

    private void OnValidate() {
        if(roomAccess == 0) name = $"Door {PositionRoomAccess()}";
        else name = $"Door {roomAccess}";
    }
}
