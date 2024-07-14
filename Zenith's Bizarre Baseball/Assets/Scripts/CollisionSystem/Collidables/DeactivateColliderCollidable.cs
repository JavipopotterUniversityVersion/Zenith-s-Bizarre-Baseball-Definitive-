using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateColliderCollidable : ICollidable
{
    [SerializeField] bool _deactivateOtherCollider = false;
    Collider2D _collider;

    private void Awake() => _collider = GetComponent<Collider2D>();

    public void Collide(Collider2D collider)
    {
        if(_deactivateOtherCollider) collider.gameObject.SetActive(false);
        else _collider.enabled = false;
    }
}