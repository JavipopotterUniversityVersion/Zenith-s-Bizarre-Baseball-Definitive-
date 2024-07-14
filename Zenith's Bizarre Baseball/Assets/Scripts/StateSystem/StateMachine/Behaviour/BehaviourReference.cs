using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourReference : MonoBehaviour, IBehaviour
{
    IBehaviour behaviour;
    string behaviourName;

    public void ExecuteBehaviour()
    {
        if(behaviour != null) behaviour.ExecuteBehaviour();
        else Debug.LogError("BehaviourReference: No behaviour set");
    }

    public void SetBehaviour(GameObject newBehaviour)
    {
        if(newBehaviour.TryGetComponent(out IBehaviour _behaviour))
        {
            behaviour = _behaviour;
            behaviourName = newBehaviour.name;
            name = behaviourName;
        }
        else Debug.LogError("BehaviourReference: No IBehaviour component found in the GameObject");
    }

    private void OnValidate() 
    {
        if(behaviour != null) name = behaviourName;
        else name = "BehaviourReference(notSet)";
    }
}