using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    MovementController movementController;
    Pointer pointer;
    WeaponHandler weaponHandler;

    private void Awake() {
        movementController = GetComponent<MovementController>();
        pointer = GetComponentInChildren<Pointer>();
        weaponHandler = GetComponentInChildren<WeaponHandler>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        movementController.Move(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context) {
        pointer.LookAt(Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()));
    }

    public void OnGamepadLook(InputAction.CallbackContext context) {
        pointer.SetPointer(context.ReadValue<Vector2>());
    }

    public void OnUse(InputAction.CallbackContext context) {
        if(context.started) weaponHandler.Use();
    }
}
