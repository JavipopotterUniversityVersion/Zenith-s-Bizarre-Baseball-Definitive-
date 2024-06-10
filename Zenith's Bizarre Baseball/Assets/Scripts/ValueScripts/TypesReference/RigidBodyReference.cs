using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyReference : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    public void SetRigidBody(Transform transform) => _rigidbody = transform.GetComponent<Rigidbody2D>();
    public void SetRigidBody(Rigidbody2D rigidbody) => _rigidbody = rigidbody;
    public Rigidbody2D GetRigidBody() => _rigidbody;
    
    public void SetKinematic(bool value) => _rigidbody.isKinematic = value;
}
