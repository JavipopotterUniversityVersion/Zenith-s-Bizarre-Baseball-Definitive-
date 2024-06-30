using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorCollidable : ICollidable
{
    [SerializeField] ObjectProcessor _operation;
    public override void OnCollide(Collider2D collision)
    {
        if(collision.TryGetComponent(out Searchable searchable))
        {
            _operation.Result(searchable.IdentifiableType.Read());
        }
    }
}
