using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class State : MonoBehaviour, IBehaviour
{
    Animator animationPlayer;
    [SerializeField] string stateAnimation = "";

    [Header("PERFORMERS")]

    [SerializeField] BehaviourPerformer[] onEnterPerformers;
    [SerializeField] BehaviourPerformer[] onUpdatePerformers;
    [SerializeField] BehaviourPerformer[] onExitPerformers;

    [Header("EXIT")]
    [SerializeField] NextStatePerformer[] nextStates;
    public NextStatePerformer[] NextStates => nextStates;

    StateHandler stateHandler;

    private void Awake()
    {
        animationPlayer = GetComponentInParent<Animator>();
        NextStatePerformer.InitializeAll(nextStates);
        stateHandler = GetComponentInParent<StateHandler>();
    }

    public void ExecuteBehaviour() => stateHandler.ChangeState(this);

    public void PlayStateAnimation()
    {
        if(animationPlayer != null && stateAnimation != "") animationPlayer.Play(stateAnimation);
    }

    public void OnStateEnter()
    {
        if(onEnterPerformers != null) Perform(onEnterPerformers);
    }

    public void OnStateUpdate()
    {
        if(onUpdatePerformers != null) Perform(onUpdatePerformers);
    }

    public void OnStateExit()
    {
        if(onExitPerformers != null) Perform(onExitPerformers);
    }

    void Perform(BehaviourPerformer[] performers)
    {
        foreach(BehaviourPerformer performer in performers)
        {
            performer.Perform();
        }
    }

    public State GetNextState() => NextStatePerformer.GetNextState(nextStates);
}

[System.Serializable]
public class NextStatePerformer
{
    [SerializeField] Condition[] conditions;
    public bool value => Condition.CheckAllConditions(conditions);
    [SerializeField] GameObject stateContainer;
    State state;

    public static State GetNextState(NextStatePerformer[] nextStates)
    {
        foreach(NextStatePerformer nextState in nextStates)
        {
            if(nextState.value) return nextState.state;
        }
        return null;
    }

    public void Initialize()
    {
        state = stateContainer.GetComponent<State>();
        Condition.InitializeAll(conditions);
    }

    public static void InitializeAll(NextStatePerformer[] nextStates)
    {
        foreach(NextStatePerformer nextState in nextStates)
        {
            nextState.Initialize();
        }
    }
}



// // [System.Serializable]
// // public class NextStatePerformerNestingConditional
// {
//     [SerializeField] Condition[] conditions;
//     public bool value => Condition.CheckAllConditions(conditions);
//     [SerializeField] NextStatePerformer[] nextStates;

//     public static State GetNextState(NextStatePerformerNestingConditional[] nextStates)
//     {
//         foreach(NextStatePerformerNestingConditional nextState in nextStates)
//         {
//             if(nextState.value) return NextStatePerformer.GetNextState(nextState.nextStates);
//         }
//         return null;
//     }

//     public void Initialize()
//     {
//         foreach(NextStatePerformer nextState in nextStates)
//         {
//             nextState.Initialize();
//         }
//     }
// }