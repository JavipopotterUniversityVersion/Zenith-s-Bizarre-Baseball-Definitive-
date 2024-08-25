using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetMainCameraLimitsCollidable : ICollidable
{
    [SerializeField] EdgeCollider2D _collider;
    public override void OnCollide(Collider2D collider)
    {
        // MainVirtualCamera.Instance.VirtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = _collider;
    }
}
