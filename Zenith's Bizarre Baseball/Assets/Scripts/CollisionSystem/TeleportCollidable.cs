using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCollidable : ICollidable
{
    [SerializeField] Transform teleportPoint;

    private void Awake()
    {
        if(teleportPoint == null) teleportPoint = transform;
    }

    public override void OnCollide(Collider2D collider) => collider.transform.position = teleportPoint.position;
}
