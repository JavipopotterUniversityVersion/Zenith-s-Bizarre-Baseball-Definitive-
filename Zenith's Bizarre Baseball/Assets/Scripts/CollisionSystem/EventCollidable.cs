using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCollidable : ICollidable
{
    [SerializeField] UnityEvent onCollideEvent;
    public UnityEvent OnCollideEvent => onCollideEvent;

    public override void OnCollide(Collider2D collision) => onCollideEvent.Invoke();
}