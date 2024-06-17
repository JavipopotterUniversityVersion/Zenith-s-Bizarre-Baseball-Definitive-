using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCollidable : ICollidable
{
    [SerializeField] Transform teleportPoint;
    [SerializeField] Transform targetTransform;

    private void Awake()
    {
        if(teleportPoint == null) teleportPoint = transform;
    }

    public override void OnCollide(Collider2D collider)
    {
        if(targetTransform == null) targetTransform = collider.transform;
        targetTransform.position = teleportPoint.position;
    }
}
