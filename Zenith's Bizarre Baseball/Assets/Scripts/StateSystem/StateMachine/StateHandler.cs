using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    [SerializeField] State initialState;
    public State InitialState => initialState;

    State _currentState;
    public State CurrentState
    {
        get => _currentState;
        private set
        {
            lastState = _currentState;
            _currentState = value;
            PlayCurrentStateAnimation();
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

    public void PlayCurrentStateAnimation() => CurrentState.PlayStateAnimation();
    public void StopHandler(float time) => StartCoroutine(StopRoutine(time));
    IEnumerator StopRoutine(float time)
    {
        enabled = false;
        yield return new WaitForSeconds(time);
        enabled = true;
    }
}
