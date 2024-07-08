using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomStateListener : MonoBehaviour
{
    RoomStateHandler _roomStateHandler;
    [SerializeField] private StateOfRoom _stateOfRoom;

    [SerializeField] UnityEvent _onStateEnter;
    [SerializeField] UnityEvent _onStateExit;

    private void Awake() 
    {
        _roomStateHandler = GetComponentInParent<RoomStateHandler>();
        _roomStateHandler.OnStateChange.AddListener(OnStateChange);
    }

    void OnStateChange(StateOfRoom previousState, StateOfRoom newState)
    {
        if (newState == _stateOfRoom)
        {
            _onStateEnter.Invoke();
        }
        else if (previousState == _stateOfRoom)
        {
            _onStateExit.Invoke();
        }
    }

    private void OnDestroy() {
        _roomStateHandler.OnStateChange.RemoveListener(OnStateChange);
    }
}
