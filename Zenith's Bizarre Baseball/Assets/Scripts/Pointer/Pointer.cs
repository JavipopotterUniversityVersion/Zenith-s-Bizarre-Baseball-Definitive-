using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Pointer : MonoBehaviour
{
    UnityEvent<Vector2> onPoint = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnPoint => onPoint;

    public void SetPointer(Vector2 vector)
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg - 90);
        onPoint.Invoke(vector);
    }

    public void LookAt(Vector2 position)
    {
        Vector2 vector = position - (Vector2)transform.position;
        vector.Normalize();

        SetPointer(vector);
    }

    public Vector2 GetDirection()
    {
        return transform.up;
    }
}
