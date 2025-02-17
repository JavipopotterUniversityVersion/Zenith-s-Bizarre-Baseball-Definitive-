using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class EventByNearestDoor : MonoBehaviour
{
    [SerializeField] UnityEvent OnDoorIsOpened = new UnityEvent();
    [SerializeField] UnityEvent OnDoorIsClosed = new UnityEvent();

    DoorIdentifier nearestDoor;
    RoomNode room;

    private void Start() {
        room = GetComponentInParent<RoomNode>();
        room.Generator.OnFinishedGeneration.AddListener(EvaluateDoorState);
    }

    void EvaluateDoorState() {
        RoomNode room = GetComponentInParent<RoomNode>();
        nearestDoor = room.Doors.OrderBy(d => Vector3.Distance(d.transform.position, transform.position)).First();

        nearestDoor.OnVerifyIdentity.AddListener(OnDoorIsOpened.Invoke);
        nearestDoor.OnFailIdentity.AddListener(OnDoorIsClosed.Invoke);
    }

    private void OnDestroy() 
    {
        if(nearestDoor != null) 
        {
            nearestDoor.OnVerifyIdentity.RemoveListener(OnDoorIsOpened.Invoke);
            nearestDoor.OnFailIdentity.RemoveListener(OnDoorIsClosed.Invoke);
        }

        if(room != null && room.Generator != null) room.Generator.OnFinishedGeneration.RemoveListener(EvaluateDoorState);
    }
}
