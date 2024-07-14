using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapperCollidable : ICollidable
{
    [SerializeField] Material _material;

    public override void OnCollide(Collider2D collider)
    {
        SpriteRenderer sr = collider.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.material = _material;
    }
}