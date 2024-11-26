using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFindingMap : MonoBehaviour
{
    [SerializeField] Identifiable targetIdentifiable;
    List<Transform> pathPoints = new List<Transform>();
    LayerMask targetLayers;

    private void Start() {
        targetLayers =  LayerMask.GetMask("Walls") | LayerMask.GetMask("Player"); 

        foreach (PathPoint child in GetComponentsInChildren<PathPoint>())
        {
            pathPoints.Add(child.transform);
        }
    }

    public Transform FindPath(Transform start, Rect rect, Transform target)
    {
        List<Transform> sortedPoints = pathPoints.OrderBy(x => Vector3.Distance(x.position, start.position)).ToList();

        List<Transform> nearestPoints = new List<Transform>();
        for(int i = 0; i < 3; i++)
        {
            RaycastHit2D otherHit = Physics2D.BoxCast(rect.position, rect.size, 0, sortedPoints[i].position - start.position, Mathf.Infinity, targetLayers);
            if(otherHit == false || otherHit.distance > Vector3.Distance(start.position, sortedPoints[i].position)) nearestPoints.Add(sortedPoints[i]);
        }

        Transform next = null;
        if(nearestPoints.Count > 0)
        {
            next = nearestPoints.OrderBy(x => Vector3.Distance(x.position, target.position)).First();
        }
        
        return next;
    }
}
