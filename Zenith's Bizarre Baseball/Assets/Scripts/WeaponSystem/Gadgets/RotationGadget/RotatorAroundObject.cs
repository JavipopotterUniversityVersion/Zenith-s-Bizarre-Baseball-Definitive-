using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorAroundObject : MonoBehaviour
{
    Transform _target;
    float _speed = 20f;
    Rigidbody2D rb;
    Vector2 orientation = new Vector2(1, -1);

    public void SetTarget(Transform target)
    {
        _target = target;
        orientation *= -1;
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        _speed = rb.velocity.magnitude;
    }

    void Update()
    {
        Vector2 direction = (_target.position - transform.position).normalized;
        direction = new Vector2(direction.y, direction.x);
        direction *= orientation;
        rb.velocity = direction * _speed;
    }
}
