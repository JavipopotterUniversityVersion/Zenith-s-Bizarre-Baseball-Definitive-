using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerSolver : MonoBehaviour
{
    [SerializeField] InputActionReference _controllerInputRef;

    private void Awake() {
        _controllerInputRef.action.started += CheckController;
    }

    private void OnDestroy() {
        _controllerInputRef.action.started -= CheckController;
    }

    void CheckController(InputAction.CallbackContext ctx)
    {
        if(ctx.action.activeControl.device is Gamepad) 
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
        else 
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
    }
}
