using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    [SerializeField] State initialState;

    State _currentState;
    State CurrentState
    {
        get => _currentState;
        set
        {
            lastState = _currentState;
            _currentState = value;
            _currentState.PlayStateAnimation();
        }
    }
    State lastState;

    [SerializeField] BehaviourPerformer[] permanentBehaviours;

    void Start()
    {
        CurrentState = initialState;
        CurrentState.OnStateEnter();
    }

    private void Update() {
        CurrentState.OnStateUpdate();

        State newState = CurrentState.GetNextState();

        if(newState != null)
        {
            ChangeState(newState);
        }

        if(permanentBehaviours != null) PerformPermanentBehaviours();
        
    }

    public void ChangeState(GameObject state) => ChangeState(state.GetComponent<State>());

    public void ChangeState(State newState)
    {
        CurrentState.OnStateExit();

        CurrentState = newState;
        
        CurrentState.OnStateEnter();
    }

    void PerformPermanentBehaviours()
    {
        foreach(BehaviourPerformer performer in permanentBehaviours)
        {
            performer.Perform();
        }
    }

    public void ReturnToLastState() => CurrentState = lastState;
    public void FullReturnToLastState() => ChangeState(lastState);
}
