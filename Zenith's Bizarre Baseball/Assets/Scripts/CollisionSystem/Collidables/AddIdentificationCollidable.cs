using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddIdentificationCollidable : ICollidable
{
    [SerializeField] private Identifiable identifiableType;
    public override void OnCollide(Collider2D collider)
    {
        if(collider.gameObject.TryGetComponent(out Searchable searchable))
        {
            if(searchable.IdentifiableType.DerivesFrom(identifiableType) == false)
            {
                collider.gameObject.AddComponent<Searchable>().SetIdentifiableType(identifiableType);
            }
        }
        else
        {
            collider.gameObject.AddComponent<Searchable>().SetIdentifiableType(identifiableType);
        }
    }
}
