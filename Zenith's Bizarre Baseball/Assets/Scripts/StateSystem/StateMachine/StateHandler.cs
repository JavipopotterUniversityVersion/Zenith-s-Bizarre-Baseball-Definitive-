using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    [SerializeField] State initialState;
    [SerializeField] State currentState;
    [SerializeField] BehaviourPerformer[] permanentBehaviours;

    void Start()
    {
        currentState = initialState;
        currentState.OnStateEnter();
        currentState.transform.gameObject.name = "current";
    }

    private void Update() {
        currentState.OnStateUpdate();

        State newState = currentState.GetNextState();

        if(newState != null)
        {
            ChangeState(newState);
        }

        if(permanentBehaviours != null) PerformPermanentBehaviours();
        
    }

    public void ChangeState(GameObject state) => ChangeState(state.GetComponent<State>());

    public void ChangeState(State newState)
    {
        currentState.OnStateExit();
        currentState.transform.gameObject.name = "passed";

        currentState = newState;
        
        currentState.OnStateEnter();
        currentState.transform.gameObject.name = "current";
    }

    void PerformPermanentBehaviours()
    {
        foreach(BehaviourPerformer performer in permanentBehaviours)
        {
            performer.Perform();
        }
    }
}
