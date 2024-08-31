using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExtendBehaviour : MonoBehaviour, IBehaviour
{
    SpriteRenderer _sr;
    [SerializeField] ObjectProcessor _speed;
    [SerializeField] BoxCollider2D[] _attachedColliders;
    LayerMask _wallLayer;

    private void Awake() 
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _wallLayer = LayerMask.GetMask("Walls");
    }

    public void ExecuteBehaviour() => StartCoroutine(Extend());

    IEnumerator Extend()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 900000, _wallLayer);

        float speed = this._speed.Result();
        float elapsedTime = 0;
        float time = Vector2.Distance(transform.position, hit.point) / speed;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            _sr.size = new Vector2(_sr.size.x, elapsedTime * speed);
            foreach(BoxCollider2D col in _attachedColliders)
            {
                col.size = new Vector2(col.size.x, _sr.size.y);
                col.offset = new Vector2(col.offset.x, _sr.size.y / 2);
            }
            yield return null;
        }
        
    }

    private void OnDrawGizmos() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 900000, LayerMask.GetMask("Walls"));
        Gizmos.color = Color.red;

        if(hit.collider != null)
        {
            Gizmos.DrawLine(transform.position, hit.point);
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 900000);
        }
    }
}
