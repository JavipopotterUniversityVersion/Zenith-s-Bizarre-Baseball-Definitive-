using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReference : MonoBehaviour, IBehaviour
{
    [SerializeField] State _state;
    public void SetReference(State state) => _state = state;
    public void ExecuteBehaviour() => _state.ExecuteBehaviour();

    private void OnValidate() {
        name = "State Reference: " + _state.name;
    }
}
