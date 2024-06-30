using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndentifiableRegisterCollidable : ICollidable, IReadable
{
    Identifiable identifiable;
    public override void OnCollide(Collider2D collision)
    {
        if(collision.TryGetComponent(out Searchable searchable))
        {
            identifiable = searchable.IdentifiableType;
        }
    }

    public float Read() => identifiable.Read();
}
