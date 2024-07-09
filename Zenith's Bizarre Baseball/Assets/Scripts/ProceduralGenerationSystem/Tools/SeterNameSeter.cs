using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeterNameSeter : MonoBehaviour
{
    private void OnValidate() 
    {
        if(GetComponentInParent<DoorIdentifier>() != null)
        {
            name = $"{GetComponentInParent<DoorIdentifier>().RoomAccess} {transform.parent.name} Seter N{transform.GetSiblingIndex()}";
        }
    }
}
