using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedReadable : MonoBehaviour, IReadable
{
    [SerializeField] Rigidbody2D rb;

    public void SetRb(Collider2D col) => rb = col.GetComponent<Rigidbody2D>();

    public float Read()
    {
        if(rb == null) return 0;
        return rb.velocity.magnitude;
    }
}