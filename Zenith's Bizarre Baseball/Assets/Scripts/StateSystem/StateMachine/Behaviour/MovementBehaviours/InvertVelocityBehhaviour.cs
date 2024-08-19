using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertVelocityBehhaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] Rigidbody2D rb;

    public void ExecuteBehaviour()
    {
        rb.velocity = -rb.velocity;
    }
}
