using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockable : MonoBehaviour
{
    [SerializeField] ObjectProcessor _reduction;
    public float Reduction => _reduction.Result();

    Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    public void Knock(Vector2 force)
    {
        rb.velocity = force * _reduction.Result();
    }
}
