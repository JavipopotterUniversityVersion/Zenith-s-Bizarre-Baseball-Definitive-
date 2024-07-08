using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStateOfRoomBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] private StateOfRoom _stateOfRoom;
    RoomStateHandler _roomStateHandler;

    private void Awake() => _roomStateHandler = GetComponentInParent<RoomStateHandler>();

    public void ExecuteBehaviour() => ChangeState(_stateOfRoom);
    void ChangeState(StateOfRoom newState)
    { 
        if(_roomStateHandler != null) _roomStateHandler.SetStateOfRoom(newState);
    }

    private void OnValidate() {
        name = "ChangeStateOfRoomBehaviour: " + _stateOfRoom.ToString();
    }
}
