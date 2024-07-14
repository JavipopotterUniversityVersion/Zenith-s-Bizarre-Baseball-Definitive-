using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCollidable : ICollidable
{
    [SerializeField] UnityEvent onCollideEvent = new UnityEvent();
    public UnityEvent OnCollideEvent => onCollideEvent;

    [SerializeField] UnityEvent<Transform> onCollideEventWithTransform = new UnityEvent<Transform>();
    public UnityEvent<Transform> OnCollideEventWithTransform => onCollideEventWithTransform;

    public override void OnCollide(Collider2D collision)
    {
        onCollideEvent.Invoke();
        onCollideEventWithTransform.Invoke(collision.transform);
    }
}