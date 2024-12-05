using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [SerializeField] Bool _boolSignal;
    CinemachineVirtualCamera _camera;
    [SerializeField] LayerMask _gameCullingMask;
    [SerializeField] LayerMask _mapCullingMask;
    Transform _originalFollow;

    private void Awake() {
        _boolSignal.OnValueChanged.AddListener(OnBoolSignalChanged);
        _camera = GetComponent<CinemachineVirtualCamera>();
        _originalFollow = _camera.Follow;
    }

    void OnBoolSignalChanged()
    {
        if (_boolSignal.Value) OpenMap();
        else CloseMap();
    }

    void OpenMap()
    {
        _camera.Follow = null;
        gameObject.layer = LayerMask.NameToLayer("Map");
        _camera.m_Lens.OrthographicSize = 60;
        Camera.main.cullingMask = _mapCullingMask;
    }

    void CloseMap()
    {
        _camera.Follow = _originalFollow;
        gameObject.layer = LayerMask.NameToLayer("Default");
        _camera.m_Lens.OrthographicSize = 18;
        Camera.main.cullingMask = _gameCullingMask;
    }
}
