using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputCondition", menuName = "Conditions/InputCondition")]
public class ScriptedInputCondition : ScriptableICondition
{
    [SerializeField] InputActionReference _inputAction;
    public void SetInputAction(InputActionReference inputAction) => _inputAction = inputAction;
    public override bool CheckCondition() => _inputAction.action.WasPerformedThisFrame();
}
