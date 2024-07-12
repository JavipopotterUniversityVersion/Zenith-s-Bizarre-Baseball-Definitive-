using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCollidersBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] Collider2D[] colliders;

    public void ExecuteBehaviour()
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = !collider.enabled;
        }
    }

    private void OnValidate() {
        if(colliders.Length == 1) name = "ToggleColliderBehaviour";
        else if(colliders.Length > 1) name = "ToggleCollidersBehaviour";
        else name = "ToggleCollidersBehaviour (No Colliders)";
    }
}
