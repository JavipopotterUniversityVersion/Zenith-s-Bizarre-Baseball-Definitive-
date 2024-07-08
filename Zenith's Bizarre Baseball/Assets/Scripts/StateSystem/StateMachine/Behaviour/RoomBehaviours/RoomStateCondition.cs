using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStateCondition : MonoBehaviour, ICondition
{
    [SerializeField] private StateOfRoom _stateOfRoom;
    RoomStateHandler _roomStateHandler;

    public bool CheckCondition() => IsState(_stateOfRoom);
    bool IsState(StateOfRoom state) => _roomStateHandler.StateOfRoom == state;

    private void OnValidate() {
        if (GetComponentInParent<RoomStateHandler>() != null)
        {
            name = "RoomState is" + _stateOfRoom.ToString();
        }
    }
}
