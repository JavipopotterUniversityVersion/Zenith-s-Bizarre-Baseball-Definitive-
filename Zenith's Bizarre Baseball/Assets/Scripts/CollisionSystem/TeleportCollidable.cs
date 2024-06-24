using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCollidable : ICollidable, IBehaviour, IGameObjectProcessor
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

    public void ExecuteBehaviour()
    {
        if(targetTransform == null) return;
        targetTransform.position = teleportPoint.position;
    }

    public void Process(GameObject gameObject)
    {
        gameObject.transform.position = teleportPoint.transform.position;
    }

    public void SetTeleportPoint(Transform teleportPoint) => this.teleportPoint = teleportPoint;
    public void SetTargetTransform(Transform targetTransform) => this.targetTransform = targetTransform;
}
