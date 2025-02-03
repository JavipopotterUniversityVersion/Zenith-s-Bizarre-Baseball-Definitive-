using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapController : MonoBehaviour
{
    [SerializeField] Bool _boolSignal;
    [SerializeField] Bool _miniMapIsActivate;
    CinemachineVirtualCamera _camera;
    [SerializeField] LayerMask _gameCullingMask;
    [SerializeField] LayerMask _mapCullingMask;
    Transform _originalFollow;

    [SerializeField] float _moveSpeed = 10;
    [SerializeField] float _zoomSpeed = 10;

    [SerializeField] InputActionReference _zoomAction;
    [SerializeField] InputActionReference _navigateAction;

    Vector2 velocity;

    private void Awake() {
        _boolSignal.OnValueChanged.AddListener(OnBoolSignalChanged);
        _camera = GetComponent<CinemachineVirtualCamera>();
        _originalFollow = _camera.Follow;

        _zoomAction.action.performed += ZoomMap;
        _navigateAction.action.performed += MoveMap;
    }

    private void OnDestroy() {
        _boolSignal.OnValueChanged.RemoveListener(OnBoolSignalChanged);
        _zoomAction.action.performed -= ZoomMap;
        _navigateAction.action.performed -= MoveMap;
    }

    private void Start() {
        CloseMap();
    }

    private void Update() {
        _camera.transform.position += new Vector3(velocity.x, velocity.y, 0) * 
        (_moveSpeed * _camera.m_Lens.OrthographicSize/160) * Time.deltaTime;
    }

    void OnBoolSignalChanged()
    {
        _miniMapIsActivate.SetValue(!_boolSignal.Value);

        if (_boolSignal.Value) OpenMap();
        else CloseMap();
    }

    void OpenMap()
    {
        _navigateAction.action.Enable();
        _zoomAction.action.Enable();

        _camera.Follow = null;
        gameObject.layer = LayerMask.NameToLayer("Map");
        _camera.m_Lens.OrthographicSize = 60;
        Camera.main.cullingMask = _mapCullingMask;
    }

    void CloseMap()
    {
        _navigateAction.action.Disable();
        _zoomAction.action.Disable();

        _camera.Follow = _originalFollow;
        gameObject.layer = LayerMask.NameToLayer("Default");
        _camera.m_Lens.OrthographicSize = 18;
        Camera.main.cullingMask = _gameCullingMask;
    }

    void MoveMap(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            velocity = context.ReadValue<Vector2>();
        }
    }

    void ZoomMap(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            float zoom = context.ReadValue<float>();
            _camera.m_Lens.OrthographicSize -= zoom * _zoomSpeed * Time.deltaTime;

            float currentZoom = _camera.m_Lens.OrthographicSize;
            _camera.m_Lens.OrthographicSize = Mathf.Clamp(currentZoom, 20, 160);
        }
    }
}
