using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DoorIdentifier : MonoBehaviour
{
    [SerializeField] private RoomAccess roomAccess;
    public RoomAccess RoomAccess => roomAccess;

    public void SetRoomAccess(RoomAccess roomAccess) => this.roomAccess = roomAccess;
    
    public bool conected => _connectedDoor != null;

    DoorIdentifier _connectedDoor;
    public DoorIdentifier ConnectedDoor
    {
        get => _connectedDoor;
        set
        {
            if(value != _connectedDoor)
            {
                if(_connectedDoor != null) _connectedDoor.SetConnectedDoor(null);
                if(value != null) value.SetConnectedDoor(this);
            }

            _connectedDoor = value;
        }
    }

    public void SetConnectedDoor(DoorIdentifier door) => _connectedDoor = door;

    private Node _DoorRoom;
    [SerializeField] UnityEvent onVerifyIdentity = new UnityEvent();
    public UnityEvent OnVerifyIdentity => onVerifyIdentity;

    [SerializeField] UnityEvent onFailIdentity = new UnityEvent();
    public UnityEvent OnFailIdentity => onFailIdentity;
    
    [SerializeField] UnityEvent onIdentifyIdentity = new UnityEvent();

    private void Awake() => StartCoroutine(Check());

    private IEnumerator Check() {
        yield return null;
        _DoorRoom = GetComponentInParent<Node>();
        _DoorRoom.Generator.OnFinishedGeneration.AddListener(CompareFlag);
    }
    private void CompareFlag() 
    {
        if (conected)
        {
            onVerifyIdentity?.Invoke();
        }
        else
        {
            onFailIdentity?.Invoke();
        }
        onIdentifyIdentity?.Invoke();
    }
}
