using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkerComponent : MonoBehaviour
{
    LinkerGadget _linkerGadget;
    Identifiable _target;

    public void SetLinkerGadget(LinkerGadget linkerGadget) => _linkerGadget = linkerGadget;
    public void SetTarget(Identifiable target) => _target = target;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.TryGetComponent(out Searchable searchable))
        {
            if(searchable.IdentifiableType.DerivesFrom(_target))
            {
                _linkerGadget.AddLinkedTarget(searchable.GetComponent<HealthHandler>());
            }
        }
    }
}
