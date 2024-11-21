using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    MovementController movementController;
    Pointer pointer;
    WeaponHandler weaponHandler;

    [SerializeField] InputActionReference _lookAction;

    private void Awake() {
        movementController = GetComponent<MovementController>();
        pointer = GetComponentInChildren<Pointer>();
        weaponHandler = GetComponentInChildren<WeaponHandler>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        movementController.Move(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context) {
        if(context.performed)
        {
            Vector2 direction = context.ReadValue<Vector2>();

            if(context.action.activeControl.device is Gamepad) 
            {
                UnityEngine.Cursor.visible = false;
                Mouse.current.WarpCursorPosition(Camera.main.WorldToScreenPoint(transform.position + new Vector3(direction.x, direction.y)));
            }

            if(UnityEngine.Cursor.lockState == CursorLockMode.Locked) pointer.SetPointer(direction);
            else pointer.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void OnGamepadLook(InputAction.CallbackContext context) {
        if(context.performed) pointer.SetPointer(context.ReadValue<Vector2>());
    }

    public void OnUse(InputAction.CallbackContext context) {
        if(context.started) weaponHandler.Use();
    }
}
