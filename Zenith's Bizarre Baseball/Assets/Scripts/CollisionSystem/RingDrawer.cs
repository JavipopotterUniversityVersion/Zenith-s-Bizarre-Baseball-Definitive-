using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RingColliderHandler))]
public class RingDrawer : MonoBehaviour
{
    RingColliderHandler _ringColliderHandler;
    LineRenderer _lineRenderer;

    private void Awake() 
    {
        _ringColliderHandler = GetComponent<RingColliderHandler>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 102;
    }

    void Update()
    {
        for(int i = 0; i < 101; i++)
        {
            float angle = i * 2 * Mathf.PI / 100;
            Vector3 pos = new Vector3(Mathf.Cos(angle) * _ringColliderHandler.Radius, Mathf.Sin(angle) * _ringColliderHandler.Radius, 0);
            pos += transform.position;
            _lineRenderer.SetPosition(i, pos);
        }

        _lineRenderer.SetPosition(101, _lineRenderer.GetPosition(0));
    }
}
