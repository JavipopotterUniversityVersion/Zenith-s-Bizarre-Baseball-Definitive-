using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomStateHandler : MonoBehaviour
{
    [SerializeField] private StateOfRoom _stateOfRoom;
    public StateOfRoom StateOfRoom => _stateOfRoom;

    UnityEvent<StateOfRoom, StateOfRoom> _onStateChange = new UnityEvent<StateOfRoom, StateOfRoom>();
    public UnityEvent<StateOfRoom, StateOfRoom> OnStateChange => _onStateChange;

    public void SetStateOfRoom(StateOfRoom state)
    {
        StateOfRoom previousState = _stateOfRoom;
        _stateOfRoom = state;
        _onStateChange.Invoke(previousState, _stateOfRoom);
    }
}

public enum StateOfRoom
{
    Unvisited,
    Visited,
    Current,
    Hostile
}
