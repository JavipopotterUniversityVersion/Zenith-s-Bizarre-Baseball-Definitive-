using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShiftableIconByInput : MonoBehaviour
{
    [SerializeField] GameObject _keyboardIconGO;
    [SerializeField] GameObject _controllerIconGO;
    [SerializeField] InputActionReference[] _actions;

    private void Start() {
        _keyboardIconGO.SetActive(true);
        _controllerIconGO.SetActive(false);
        
        foreach (var action in _actions) {
            action.action.started += CheckController;
            action.action.Enable();
        }
    }

    private void OnDestroy() {
        foreach (var action in _actions) {
            action.action.started -= CheckController;
            action.action.Disable();
        }
    }

    private void CheckController(InputAction.CallbackContext ctx)
    {
        if(ctx.action.activeControl.device is Gamepad) {
            _keyboardIconGO.SetActive(false);
            _controllerIconGO.SetActive(true);
        } else {
            _keyboardIconGO.SetActive(true);
            _controllerIconGO.SetActive(false);
        }
    }
}
