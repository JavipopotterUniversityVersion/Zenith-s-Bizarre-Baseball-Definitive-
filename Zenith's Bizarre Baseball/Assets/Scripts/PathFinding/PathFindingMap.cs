using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFindingMap : MonoBehaviour
{
    [SerializeField] Identifiable targetIdentifiable;
    Transform target;
    Transform[] pathPoints;

    private void Start() {
        target = SearchManager.Instance.GetClosestSearchable(transform.position, targetIdentifiable);
        pathPoints = transform.Cast<Transform>().ToArray();
    }

    public void FindPath() {
        List<Transform> path = new List<Transform>();
        Transform current = transform;
        while (current != target) {
            Transform next = pathPoints.OrderBy(p => Vector3.Distance(p.position, current.position)).First();
            path.Add(next);
            current = next;
        }
        path.Add(target);
        pathPoints = path.ToArray();
    }
}
