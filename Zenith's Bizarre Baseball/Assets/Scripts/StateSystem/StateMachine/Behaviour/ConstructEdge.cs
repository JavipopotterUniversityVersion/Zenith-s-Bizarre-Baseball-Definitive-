using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class ConstructEdge: MonoBehaviour
{
    EdgeCollider2D _edgeCollider;

    private void Awake() {
        _edgeCollider = GetComponent<EdgeCollider2D>();
    }

    public void ConstructBySearchable(Identifiable type)
    {
        List<Vector2> points = new List<Vector2>();
        foreach (Transform searchable in SearchManager.Instance.GetAllSearchables<Transform>(type))
        {
            points.Add(searchable.position);
        }
        _edgeCollider.points = points.ToArray();
    }

    public void ConstructByObjects(GameObject[] objects)
    {
        List<Vector2> points = new List<Vector2>();
        foreach (GameObject obj in objects)
        {
            points.Add(obj.transform.position);
        }
        _edgeCollider.points = points.ToArray();
    }
}
