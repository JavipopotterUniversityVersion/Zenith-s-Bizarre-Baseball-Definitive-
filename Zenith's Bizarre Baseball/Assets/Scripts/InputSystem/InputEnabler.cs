using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputEnabler", menuName = "InputSystem/InputEnabler")]
public class InputEnabler : ScriptableObject
{
    public void EnableInput(InputActionReference inputActionReference) => inputActionReference.action.Enable();
    public void DisableInput(InputActionReference inputActionReference) => inputActionReference.action.Disable();
}
