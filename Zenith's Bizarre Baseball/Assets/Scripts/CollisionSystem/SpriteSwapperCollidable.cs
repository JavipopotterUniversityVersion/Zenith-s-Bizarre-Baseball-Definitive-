using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapperCollidable : ICollidable
{
    [SerializeField] Sprite sprite;

    public override void OnCollide(Collider2D collision)
    {
        collision.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
    }
}
