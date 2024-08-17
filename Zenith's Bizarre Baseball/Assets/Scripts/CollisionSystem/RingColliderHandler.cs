using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingColliderHandler : MonoBehaviour
{
    [SerializeField] CollidableGroup[] _onEnterCollidables;
    [SerializeField] CollidableGroup[] _onStayCollidables;
    [SerializeField] CollidableGroup[] _onExitCollidables;

    [SerializeField] ObjectProcessor _radius;
    public float Radius => _radius.Result();

    [SerializeField] float width;
    public float Width => width;

    List<Collider2D> _collidersInside = new List<Collider2D>();

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius.Result() - width);
        Gizmos.DrawWireSphere(transform.position, _radius.Result() + width);
    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius.Result());
        foreach(Collider2D collider in colliders)
        {
            if(Vector2.Distance(transform.position, collider.transform.position) > _radius.Result() - width
            && Vector2.Distance(transform.position, collider.transform.position) < _radius.Result() + width)
            {
                if(!_collidersInside.Contains(collider))
                {
                    _collidersInside.Add(collider);
                    CollidableGroup.CheckCollidables(_onEnterCollidables, collider);
                }
                else
                {
                    CollidableGroup.CheckCollidables(_onStayCollidables, collider);
                }
            }
            else
            {
                if(_collidersInside.Contains(collider))
                {
                    _collidersInside.Remove(collider);
                    CollidableGroup.CheckCollidables(_onExitCollidables, collider);
                }
            }
        }
    }
}
