using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapperCollidable : ICollidable, IGameObjectProcessor
{
    [SerializeField] UnityEngine.Sprite sprite;

    public override void OnCollide(Collider2D collision)
    {
        collision.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
    }

    public void Process(GameObject gameObject)
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
    }
}
