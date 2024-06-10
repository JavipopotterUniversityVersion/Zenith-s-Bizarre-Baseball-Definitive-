using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformReference : MonoBehaviour
{
    Transform _transform;

    public void SetTransform(Transform transform) => _transform = transform;
    public Transform GetTransform() => _transform;

    public void SetTransformPosition(Transform transform) => _transform.position = transform.position;
    public void SetTransformPosition(Vector2 position) => _transform.position = position;
    public void SetTransformParent(Transform parent) => _transform.SetParent(parent);
}
