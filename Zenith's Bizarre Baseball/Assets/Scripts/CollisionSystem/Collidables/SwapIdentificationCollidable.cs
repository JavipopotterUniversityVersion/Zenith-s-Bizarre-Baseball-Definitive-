using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapIdentificationCollidable : ICollidable
{
    [SerializeField] Identifiable _fromIdentifiable;
    [SerializeField] Identifiable _toIdentifiable;

    public override void OnCollide(Collider2D collider)
    {
        foreach(Searchable searchable in collider.gameObject.GetComponents<Searchable>())
        {
            if(searchable.IdentifiableType.DerivesFrom(_fromIdentifiable))
            {
                searchable.SetIdentifiableType(_toIdentifiable);
            }
        }
    }
}
