using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(RaycastHandler))]
public class RayDrawer : MonoBehaviour
{
    LineRenderer lineRenderer;
    RaycastHandler raycastHandler;

    private void Awake() 
    {
        lineRenderer = GetComponent<LineRenderer>();
        raycastHandler = GetComponent<RaycastHandler>();
        lineRenderer.positionCount = 2;
    }

    private void Update() 
    {
        if(raycastHandler.Checking)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, raycastHandler.LastHitPoint);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
