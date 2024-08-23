using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MainVirtualCamera : MonoBehaviour
{
    public static MainVirtualCamera Instance { get; private set; }
    
    CinemachineVirtualCamera _virtualCamera;
    public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

    private void Awake() {
        Instance = this;
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
}
