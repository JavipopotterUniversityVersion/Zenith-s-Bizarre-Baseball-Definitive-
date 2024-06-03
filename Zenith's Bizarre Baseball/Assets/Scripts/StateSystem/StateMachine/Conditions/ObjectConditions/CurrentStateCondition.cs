using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentStateCondition : MonoBehaviour, ICondition
{
    StateHandler stateHandler;
    [SerializeField] State stateToCheck;

    private void Awake() {
        stateHandler = GetComponentInParent<StateHandler>();
    }

    public bool CheckCondition()=> stateHandler.CurrentState == stateToCheck;

    private void OnValidate() {
        stateHandler = GetComponentInParent<StateHandler>();
        if(stateHandler == null) return;

        if(stateToCheck != null) name = "CurrentState is " + stateToCheck.name + "?";
        else name = "CurrentState is null?";
    }
}