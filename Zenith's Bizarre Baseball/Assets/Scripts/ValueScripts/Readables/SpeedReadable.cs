using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedReadable : Readable
{
    [SerializeField] Rigidbody2D rb;

    public void SetRb(Collider2D col) => rb = col.GetComponent<Rigidbody2D>();

    public override float Read()
    {
        print(rb.velocity.magnitude);
        return rb.velocity.magnitude;
    }
}