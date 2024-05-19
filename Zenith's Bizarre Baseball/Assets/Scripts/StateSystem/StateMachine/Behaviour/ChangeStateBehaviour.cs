using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeStateBehaviour : MonoBehaviour, IBehaviour
{
    StateHandler stateHandler;
    [SerializeField] State stateToChangeTo;

    private void Awake() {
        stateHandler = GetComponentInParent<StateHandler>();
    }

    public void ExecuteBehaviour()
    {
        print("Changing state to " + stateToChangeTo.name);
        stateHandler.ChangeState(stateToChangeTo);
    }

    private void OnValidate() {
        if(stateToChangeTo != null)
            name = "ChangeToState => " + stateToChangeTo.name;
    }
}