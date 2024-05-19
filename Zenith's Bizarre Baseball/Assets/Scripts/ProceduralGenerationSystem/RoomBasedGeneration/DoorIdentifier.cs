using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DoorIdentifier : MonoBehaviour
{
    private RoomAccess roomAccess;
    private Room _DoorRoom;
    [SerializeField] UnityEvent onVerifyIdentity = new UnityEvent();
    [SerializeField] UnityEvent onFailIdentity = new UnityEvent();

    private void Start() => CompareFlag();
    private void Awake()
    {
        _DoorRoom = GetComponentInParent<Room>();   
    }
    private void CompareFlag() 
    {
        roomAccess = PositionRoomAccess();
        if (_DoorRoom.totalAccess.HasFlag(roomAccess))
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
}
