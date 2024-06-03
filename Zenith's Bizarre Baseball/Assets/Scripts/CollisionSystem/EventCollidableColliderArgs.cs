using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCollidableColliderArgs : ICollidable
{
    [SerializeField] UnityEvent<Collider2D> _onCollide;

    public override void OnCollide(Collider2D col) => _onCollide.Invoke(col);
}
