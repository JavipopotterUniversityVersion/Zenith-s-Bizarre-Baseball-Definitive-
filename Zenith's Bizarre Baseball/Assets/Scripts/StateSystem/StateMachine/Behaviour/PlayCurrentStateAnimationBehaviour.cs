using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCurrentStateAnimationBehaviour : MonoBehaviour, IBehaviour
{
    StateHandler stateHandler;

    private void Awake() {
        stateHandler = GetComponentInParent<StateHandler>();
    }

    public void ExecuteBehaviour()
    {
        stateHandler.PlayCurrentStateAnimation();
    }

    private void OnValidate() {
        stateHandler = GetComponentInParent<StateHandler>();
        if(stateHandler == null) return;

        if(stateHandler.CurrentState != null)
            name = "PlayCurrentStateAnimation => " + stateHandler.CurrentState.name;
        else
            name = "PlayCurrentStateAnimation";
    }
}
