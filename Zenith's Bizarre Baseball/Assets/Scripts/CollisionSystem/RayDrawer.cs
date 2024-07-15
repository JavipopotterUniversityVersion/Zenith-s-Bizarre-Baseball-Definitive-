using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(RaycastHandler))]
public class RayDrawer : MonoBehaviour
{
    LineRenderer lineRenderer;
    RaycastHandler raycastHandler;

    [SerializeField] DrawingSettings _drawRay;
    [SerializeField] DrawingSettings _hitRay;

    private void Awake() 
    {
        lineRenderer = GetComponent<LineRenderer>();
        raycastHandler = GetComponent<RaycastHandler>();
        lineRenderer.positionCount = 2;
    }

    private void Update() 
    {
        DrawingSettings _currentDrawSettings;

        if(raycastHandler.Checking) _currentDrawSettings = _hitRay;
        else if(raycastHandler.Drawing) _currentDrawSettings = _drawRay;
        else
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = 2;

        lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, _currentDrawSettings.width);
        lineRenderer.colorGradient = _currentDrawSettings.color;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, raycastHandler.LastHitPoint);
    }

    [System.Serializable]
    struct DrawingSettings
    {
        public Gradient color;
        public float width;
    }
}
