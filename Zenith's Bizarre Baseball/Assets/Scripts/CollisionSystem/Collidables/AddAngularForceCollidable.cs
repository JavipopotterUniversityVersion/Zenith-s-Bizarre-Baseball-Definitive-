using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAngularForceCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _angularForce;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.AddTorque(_angularForce.Result());
        }
    }
}