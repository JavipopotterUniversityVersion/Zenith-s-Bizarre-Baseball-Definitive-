using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MainVirtualCamera : MonoBehaviour
{
    static MainVirtualCamera _instance;
    public static MainVirtualCamera Instance => _instance;
    public CinemachineTargetGroup TargetGroup;
    
    private void Awake() 
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
