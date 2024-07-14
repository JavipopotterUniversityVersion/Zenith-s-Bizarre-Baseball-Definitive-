using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBehaviourReferenceBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] BehaviourReference behaviourReference;
    [SerializeField] GameObject behaviour;

    public void ExecuteBehaviour()
    {
        behaviourReference.SetBehaviour(behaviour);
    }

    private void OnValidate()
    {
        if(behaviour != null)
        {
            name = $"Set Behaviour Reference: {behaviour.name}";
        }
    }
}
