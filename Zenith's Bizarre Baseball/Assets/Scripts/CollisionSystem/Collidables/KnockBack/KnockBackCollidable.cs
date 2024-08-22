using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackCollidable : ICollidable
{
    [SerializeField] ObjectProcessor objectProcessor;

    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out Knockable knockable))
        {
            knockable.Knock(objectProcessor.Result() * (collider.transform.position - transform.position).normalized);
        }
    }
}