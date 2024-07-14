using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCircleHandler : MonoBehaviour
{
     [SerializeField] CollidableGroup[] OnEnterCollidables;
    public CollidableGroup[] OnEnterCollidablesProp() => OnEnterCollidables;

    [SerializeField] float radius;
    [SerializeField] Vector2 offset;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + offset, radius);
    }

    void CheckCollidables(CollidableGroup[] collidables, Collider2D collision)
    {
        foreach (var collidableGroup in collidables)
        {
            if(collidableGroup.Layer == (collidableGroup.Layer | (1 << collision.gameObject.layer)))
            {
                foreach (var collidable in collidableGroup.Collidables)
                {
                    collidable.OnCollide(collision);
                }
            }
        }
    }

    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + offset, radius);

        if(colliders != null)
        {
            foreach (var collider in colliders)
            {
                CheckCollidables(OnEnterCollidables, collider);
            }
        }
    }
}
